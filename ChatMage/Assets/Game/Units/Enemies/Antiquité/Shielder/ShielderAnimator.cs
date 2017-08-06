using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShielderAnimator : MonoBehaviour
{
    [Header("Linking")]
    public Transform shield;
    public Transform sword;
    public Collider2D swordTrigger;

    [Header("Shield To The Left")]
    public Vector2 shieldFinalPos;
    public float shieldFinalRot;
    public float shieldMoveDuration;
    public float pause;

    [Header("Attack")]
    public float swordForwardDeltaX = 0.5f;
    public float swordForwardDuration = 0.2f;
    public float swordBackwardDuration = 0.2f;

    public Tween AttackAnimation()
    {
        Sequence sq = DOTween.Sequence();

        Vector2 originalShieldPos = shield.localPosition;
        float originalShieldRot = shield.localRotation.eulerAngles.z;
        float originalSwordX = sword.localPosition.x;

        //Tasse le shield a gauche
        sq.Append(shield.DOLocalMove(shieldFinalPos, shieldMoveDuration).SetEase(Ease.InOutSine));
        sq.Join(shield.DOLocalRotate(Vector3.forward * shieldFinalRot, shieldMoveDuration).SetEase(Ease.InOutSine));

        //Activate sword trigger
        sq.AppendCallback(delegate ()
        {
            swordTrigger.enabled = true;
        });

        sq.AppendInterval(pause);

        //sword forward
        sq.Append(
            sword.DOLocalMoveX(sword.localPosition.x + swordForwardDeltaX, swordForwardDuration).SetEase(Ease.InSine));

        //backward
        sq.Append(
            sword.DOLocalMoveX(originalSwordX, swordBackwardDuration).SetEase(Ease.InOutSine));

        //Deactivate sword trigger midway in retract
        sq.AppendCallback(delegate ()
        {
            swordTrigger.enabled = false;
        });

        sq.Append(shield.DOLocalMove(originalShieldPos, shieldMoveDuration).SetEase(Ease.InOutSine));
        sq.Join(shield.DOLocalRotate(Vector3.forward * originalShieldRot, shieldMoveDuration).SetEase(Ease.InOutSine));

        //Deactivate sword trigger IF CANCELLED
        sq.OnKill(delegate ()
        {
            if (swordTrigger != null)
                swordTrigger.enabled = false;
        });


        return sq;
    }
}
