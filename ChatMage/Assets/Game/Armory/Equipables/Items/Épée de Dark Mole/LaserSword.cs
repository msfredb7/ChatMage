using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserSword : MonoBehaviour
{
    public SimpleColliderListener colliderListener;
    [Forward]
    public Targets targets;

    [Header("Hit")]
    public AudioPlayable onHitSFX;

    [Header("Animation")]
    public float swordFinalLength = 0.75f;
    public Ease openEase = Ease.Linear;
    public float openDuration = 0.75f;
    public Ease closeEase = Ease.Linear;
    public float closeDuration = 1f;

    [Header("Collider")]
    public BoxCollider2D boxCollider;

    [Header("Laser sprite")]
    public SpriteRenderer laser;
    public float laserBaseSize = 0.32f;

    private Tween tween;
    private float timescale = 1;

    void Awake()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
        CloseInstant();
    }

    void SetTweenIfNotSet()
    {
        if (tween != null)
            return;
        float val = 0;
        Vector2 boxSize = boxCollider.size;
        Vector2 boxOffset = Vector2.zero;
        Vector2 laserSize = laser.size;
        Vector3 laserOffset = Vector3.zero;
        Transform laserTr = laser.transform;
        tween = DOTween.To(() => val,
            (x) =>
            {
                val = x;
                laserSize.x = laserBaseSize + val * swordFinalLength;
                boxSize.x = val * swordFinalLength;
                boxOffset.x = boxSize.x / 2;
                laserOffset.x = boxOffset.x;

                boxCollider.size = boxSize;
                boxCollider.offset = boxOffset;

                laser.size = laserSize;
                laserTr.localPosition = laserOffset;
            },
            1,
            openDuration).SetEase(openEase);
        tween.SetAutoKill(false);
        ApplyTimescale();
    }

    public void Open(TweenCallback onComplete)
    {
        SetTweenIfNotSet();

        tween.PlayForward();
        tween.OnComplete(onComplete);
    }
    public void OpenInstant()
    {
        SetTweenIfNotSet();

        tween.PlayForward();
        tween.Goto(tween.Duration());
    }

    public void Close(TweenCallback onComplete)
    {
        SetTweenIfNotSet();
        
        tween.PlayBackwards();
        tween.OnRewind(onComplete);
    }
    public void CloseInstant()
    {
        SetTweenIfNotSet();

        tween.PlayBackwards();
        tween.Goto(0);
    }

    void OnDestroy()
    {
        Kill();
    }

    private void Kill()
    {
        if (tween != null && tween.IsActive())
            tween.Kill();
        tween = null;
    }

    public void UpdateTimescale(float timescale)
    {
        this.timescale = timescale;
        ApplyTimescale();
    }

    private void ApplyTimescale()
    {
        if (tween != null)
            tween.timeScale = timescale;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if (unit != null)
        {
            IAttackable attackable = unit as IAttackable;
            if (attackable != null)
            {
                if (targets.IsValidTarget(unit))
                {
                    //Attack !
                    bool wasDead = unit.IsDead;

                    PlayerController player = Game.instance != null ? Game.instance.Player : null;
                    attackable.Attacked(other, 1, player != null ? player.vehicle : null, listener.info);
                    if (!wasDead && unit.IsDead)
                    {
                        if (player != null)
                            player.playerStats.RegisterKilledUnit(unit);
                    }

                    SoundManager.PlaySFX(onHitSFX);
                }
            }
        }
    }
}
