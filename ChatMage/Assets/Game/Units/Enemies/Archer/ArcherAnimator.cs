using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcherAnimator : MonoBehaviour
{
    [Header("Reload")]
    public float reloadDuration = 2;
    public SpriteRenderer spriteRenderer;
    public Color clipFull;
    public Color clipEmpty;

    [Header("Shoot")]
    public Transform arm;
    public Transform arrowLaunchEmitter;
    public float armRetractX = -0.5f;
    public float armShootX = 1;

    [Header("Shoot Durations")]
    public float armRetractDuration = 0.7f;
    public float armShootDuration = 0.1f;
    public float armResetDuration = 0.25f;

    void Awake()
    {
        ArcherVehicle archer = GetComponent<ArcherVehicle>();
        if (archer.Ammo == 0)
            AsNoAmmo();
    }

    #region Reload

    public Tween ReloadAnimation()
    {
        spriteRenderer.color = clipEmpty;
        return spriteRenderer.DOColor(clipFull, reloadDuration);
    }

    public void CancelReload()
    {
        AsNoAmmo();
    }

    public void AsNoAmmo()
    {
        spriteRenderer.color = clipEmpty;
    }

    #endregion

    #region Shoot

    public Tween ShootAnim(TweenCallback onShootMoment)
    {
        Sequence sq = DOTween.Sequence();

        //charge
        sq.Append(arm.DOLocalMoveX(armRetractX, armRetractDuration).SetEase(Ease.OutSine));

        //Shoot
        sq.Append(arm.DOLocalMoveX(armShootX, armShootDuration).SetEase(Ease.InSine));
        sq.AppendCallback(onShootMoment);

        //retract
        sq.Append(arm.DOLocalMoveX(0, armResetDuration).SetEase(Ease.InOutSine));

        return sq;
    }

    #endregion
}
