using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimatorV2 : EnemyAnimator
{
    public SpriteRenderer[] arrowRenderer;

    int attackHash = Animator.StringToHash("attack");
    int cancelReloadHash = Animator.StringToHash("cancelReload");
    int moveSpeedHash = Animator.StringToHash("moveSpeed");
    int reloadHash = Animator.StringToHash("reload");

    Action shootCallback;
    Action shootMoment;
    Action reloadCallback;
    Action reloadMoment;

    ArcherVehicle veh;
    void Awake()
    {
        veh = GetComponent<ArcherVehicle>();
    }
    protected override EnemyVehicle Vehicle { get { return veh; } }

    protected override void Start()
    {
        base.Start();
        OnAmmoChange();
        veh.onAmmoChange += OnAmmoChange;
    }

    public void FleeAnimation()
    {
        controller.SetFloat(moveSpeedHash, veh.fleeMoveSpeed / veh.walkMoveSpeed);
    }
    public void StopFleeAnimation()
    {
        controller.SetFloat(moveSpeedHash, 1);
    }

    public void ShootAnimation(Action shootMoment, Action onComplete)
    {
        this.shootMoment = shootMoment;
        shootCallback = onComplete;
        controller.SetTrigger(attackHash);
    }

    public void CancelReload()
    {
        controller.SetTrigger(cancelReloadHash);
    }

    public void ReloadAnimation(Action reloadMoment, Action onComplete)
    {
        this.reloadCallback = onComplete;
        this.reloadMoment = reloadMoment;
        controller.SetTrigger(reloadHash);
    }

    private void OnAmmoChange()
    {
        bool enabled = veh.Ammo > 0;
        foreach (var spr in arrowRenderer)
        {
            spr.enabled = enabled;
        }
    }

    private void _ShootMoment()
    {
        if (shootMoment != null)
            shootMoment();
        shootMoment = null;
    }

    private void _ShootComplete()
    {
        if (shootCallback != null)
            shootCallback();
        shootCallback = null;
    }

    private void _ReloadMoment()
    {
        if (reloadMoment != null)
        {
            reloadMoment();
        }
        reloadMoment = null;
    }

    private void _ReloadComplete()
    {
        if (reloadCallback != null)
        {
            reloadCallback();
        }
        reloadCallback = null;
    }
}
