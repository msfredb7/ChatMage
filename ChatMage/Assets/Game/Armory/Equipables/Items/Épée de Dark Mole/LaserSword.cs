using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserSword : MonoBehaviour
{
    public SimpleColliderListener colliderListener;
    [Forward]
    public Targets targets;

    [Header("Animation")]
    public Transform laserAnchor;
    public Ease openEase = Ease.Linear;
    public float openDuration = 0.75f;
    public Ease closeEase = Ease.Linear;
    public float closeDuration = 1f;

    private Tween tween;
    private float timescale = 1;

    void Awake()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
        CloseInstant();
    }

    public void OpenSword(TweenCallback onComplete)
    {
        Kill();
        tween = laserAnchor
            .DOScaleX(1, openDuration)
            .SetEase(openEase)
            .OnComplete(onComplete);
        ApplyTimescale();
    }
    public void OpenInstant()
    {
        Kill();
        laserAnchor.localScale = Vector3.one;
    }

    public void CloseSword(TweenCallback onComplete)
    {
        Kill();
        tween = laserAnchor
            .DOScaleX(0, closeDuration)
            .SetEase(closeEase)
            .OnComplete(onComplete);
        ApplyTimescale();
    }
    public void CloseInstant()
    {
        Kill();
        laserAnchor.localScale = new Vector3(0, 1, 1);
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
                }
            }
        }
    }
}
