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
    public float hitCameraShake = 0.5f;

    [Header("Break Animation")]
    public float break_inheritedVelocity = 0.5f;
    public float break_minDistance = 0.5f;
    public float break_maxDistance = 1.25f;
    public float break_minTwist = 10;
    public float break_maxTwist = 35;
    public float break_minDuration = 0.3f;
    public float break_maxDuration = 0.4f;
    public Ease break_moveEase = Ease.Linear;
    public Ease break_twistEase = Ease.OutSine;

    [Header("Open/Close Animation")]
    public float swordFinalLength = 0.75f;
    public Ease openEase = Ease.Linear;
    public float openDuration = 0.75f;
    public float laserBaseSize = 0.32f;

    [Header("Collider")]
    public BoxCollider2D boxCollider;

    [Header("Sprites")]
    public SpriteRenderer laser;
    public SpriteRenderer handleSprite;


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

    public void Close()
    {
        Close(null);
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

    public void BreakOffAndClose(Vector2 inherentVelocity, Transform newParent, TweenCallback onComplete)
    {
        Transform tr = transform;
        tr.SetParent(newParent);

        float animDuration = Random.Range(break_minDuration, break_maxDuration);
        float moveDistance = Random.Range(break_minDistance, break_maxDistance);
        float twist = Random.Range(break_minTwist, break_maxTwist) * (Random.value > 0.5f ? -1 : 1);

        Sequence sequence = DOTween.Sequence();

        Vector2 distByInherentVelocity = inherentVelocity * break_inheritedVelocity * animDuration;
        Vector2 distByBreak = tr.right * moveDistance;

        sequence.Join(tr.DOLocalMove((Vector2)tr.position + distByInherentVelocity + distByBreak, animDuration).SetEase(break_moveEase));
        sequence.Join(tr.DORotate(new Vector3(0, 0, twist), animDuration, RotateMode.LocalAxisAdd)
            .SetEase(break_twistEase));
        sequence.AppendInterval(0.25f);
        sequence.AppendCallback(Close);
        sequence.AppendInterval(openDuration);
        sequence.AppendCallback(LightOff);
        sequence.Append(handleSprite.DOFade(0, 0.5f));
        sequence.OnComplete(onComplete);
    }

    private void LightOn()
    {
        laser.enabled = true;
        boxCollider.enabled = true;
    }
    private void LightOff()
    {
        laser.enabled = false;
        boxCollider.enabled = false;
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

                    PlayerController player = Game.Instance != null ? Game.Instance.Player : null;
                    attackable.Attacked(other, 1, player != null ? player.vehicle : null, listener.info);
                    if (!wasDead && unit.IsDead)
                    {
                        if (player != null)
                            player.playerStats.RegisterKilledUnit(unit);
                    }

                    SoundManager.PlaySFX(onHitSFX);
                    Vector2 v = (player.vehicle.Position - unit.Position).normalized;
                    Game.Instance.gameCamera.vectorShaker.Hit(v * hitCameraShake);
                }
            }
        }
    }
}
