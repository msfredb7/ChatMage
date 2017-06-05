using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShielderAnimator : MonoBehaviour
{
    [Header("Linking")]
    public ShielderVehicle vehicle;
    public Transform shield;
    public Transform sword;
    public Collider2D swordTrigger;

    [Header("Shield Bump")]
    public float bumpDeltaX = 0.25f;
    public float bumpForwardDuration = 0.25f;
    public float bumpBackwardDuration = 0.25f;

    [Header("Attack")]
    public float swordForwardDeltaX = 0.5f;
    public float swordForwardDuration = 0.2f;
    public float swordBackwardDuration = 0.2f;

    private float originalShieldX;
    private float originalSwordX;
    private Tween tween;
    private bool isBumping = false;
    private bool isAttacking = false;

    void Awake()
    {
        vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
        originalShieldX = shield.localPosition.x;
        originalSwordX = sword.localPosition.x;
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        SetTimeScale();
    }

    void SetTimeScale()
    {
        if (tween != null)
            tween.timeScale = vehicle.TimeScale;
    }

    public bool IsBumping { get { return isBumping; } }
    public bool IsAttacking { get { return isAttacking; } }

    public void BumpShield(Action callback, float callbackDelay = 0)
    {
        shield.DOKill();

        isBumping = true;
        Sequence sq = DOTween.Sequence();

        //forward
        sq.Append(
            shield.DOLocalMoveX(shield.localPosition.x + bumpDeltaX, bumpForwardDuration).SetEase(Ease.InSine));

        //backward
        sq.Append(
            shield.DOLocalMoveX(originalShieldX, bumpBackwardDuration).SetEase(Ease.InOutSine));


        sq.InsertCallback(bumpForwardDuration + callbackDelay,
            delegate ()
            {
                if (callback != null)
                    callback();
            });

        sq.SetUpdate(false).OnComplete(delegate ()
        {
            isBumping = false;
        });

        tween = sq;

        SetTimeScale();
    }

    public void Attack()
    {
        sword.DOKill();

        isAttacking = true;
        Sequence sq = DOTween.Sequence();

        //forward
        sq.Append(
            sword.DOLocalMoveX(sword.localPosition.x + swordForwardDeltaX, swordForwardDuration).SetEase(Ease.InSine));

        //Activate sword trigger
        sq.InsertCallback(swordForwardDuration / 2, delegate ()
          {
              swordTrigger.enabled = true;
          });

        //backward
        sq.Append(
            sword.DOLocalMoveX(originalSwordX, swordBackwardDuration).SetEase(Ease.InOutSine));

        //Deactivate sword trigger midway in retract
        sq.InsertCallback(swordForwardDuration + swordBackwardDuration / 2, delegate ()
        {
            swordTrigger.enabled = false;
        });

        //Deactivate sword trigger IF CANCELLED
        sq.OnKill(delegate ()
        {
            swordTrigger.enabled = false;
        });

        sq.SetUpdate(false).OnComplete(delegate () { isAttacking = false; });

        tween = sq;

        SetTimeScale();
    }

    public void BringOutShield()
    {
        shield.gameObject.SetActive(true);
    }

    public void HideShield()
    {
        shield.gameObject.SetActive(false);
    }
}
