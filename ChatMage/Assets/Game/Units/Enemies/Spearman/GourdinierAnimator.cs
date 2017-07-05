using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GourdinierAnimator : MonoBehaviour
{
    [Header("Linking")]
    public GourdinierVehicle vehicle;
    public Transform gourdin;

    [Header("Settings")]
    [Header("Charge")]
    public float chargeDuration = 0.75f;
    public float gourdinXCharge;
    [Header("Attack")]
    public SimpleColliderListener attackListener;
    public Collider2D attackTrigger;
    public float gourdinXAttack;
    public float attackDuration = 0.75f;
    public float bumpForce = 5;

    Tween tween;
    bool hasHitInThisAttack = false;

    void Awake()
    {
        vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
        attackListener.onTriggerEnter += AttackListener_onTriggerEnter;
    }

    void SetTimeScale()
    {
        if (tween != null)
            tween.timeScale = vehicle.TimeScale;
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        SetTimeScale();
    }

    public void Cancel()
    {

    }

    public void Charge(TweenCallback onComplete)
    {
        //Launch anim
        Sequence sq = DOTween.Sequence();

        //Gourdin move
        sq.Join(
            gourdin.DOLocalMoveX(gourdinXCharge, chargeDuration)
            .SetEase(Ease.OutSine).SetUpdate(false));

        sq.OnComplete(onComplete).SetUpdate(false);

        tween = sq;

        SetTimeScale();
    }

    public void Attack(TweenCallback onComplete)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(
            gourdin.DOLocalMoveX(gourdinXAttack, attackDuration).SetEase(Ease.InSine).SetUpdate(false));

        //Activate trigger after 33% of the attack move
        sq.InsertCallback(attackDuration * 0.33f,
            delegate ()
        {
            hasHitInThisAttack = false;
            attackTrigger.enabled = true;
        });

        //Retrack gourdin
        sq.Append(
            gourdin.DOLocalMoveX(0, 0.25f).SetEase(Ease.InOutSine).SetUpdate(false));

        //Disable trigger
        sq.AppendCallback(delegate ()
        {
            attackTrigger.enabled = false;
        });

        sq.OnComplete(onComplete).SetUpdate(false);

        tween = sq;

        SetTimeScale();
    }

    private void AttackListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (hasHitInThisAttack)
            return;

        if (vehicle.IsValidTarget(other.parentUnit.allegiance))
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, vehicle, listener.info);
                hasHitInThisAttack = true;

                //Bump
                if (other.parentUnit is Vehicle)
                {
                    Vector2 v = other.parentUnit.Position - vehicle.Position;
                    (other.parentUnit as Vehicle).Bump(v.normalized * bumpForce, -1, BumpMode.VelocityAdd);
                }
            }
        }
    }
}
