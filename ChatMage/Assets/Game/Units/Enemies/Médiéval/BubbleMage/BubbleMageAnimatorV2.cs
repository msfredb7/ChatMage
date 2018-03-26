using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMageAnimatorV2 : EnemyAnimator
{
    protected BubbleMageVehicle veh;
    protected override EnemyVehicle Vehicle { get { return veh; } }

    private int attackHash = Animator.StringToHash("attack");
    private int cancelHash = Animator.StringToHash("cancel");

    private Action chargeMoment;
    private Action shootMoment;
    private Action shootCallback;

    public AudioPlayable chargeSound;
    public AudioPlayable shootSound;

    void Awake()
    {
        veh = GetComponent<BubbleMageVehicle>();
    }

    public void AttackAnimation(Action chargeCompleteMoment, Action shootMoment, Action onComplete)
    {
        chargeMoment = chargeCompleteMoment;
        this.shootMoment = shootMoment;
        shootCallback = onComplete;
        DefaultAudioSources.PlaySFX(chargeSound,0,1,GetComponent<AudioSource>());
        controller.SetTrigger(attackHash);
    }

    public void CancelAttack()
    {
        controller.SetTrigger(cancelHash);
    }

    private void _ChargeMoment()
    {
        if (chargeMoment != null)
            chargeMoment();
        chargeMoment = null;
    }

    private void _ShootMoment()
    {
        DefaultAudioSources.PlaySFX(shootSound);
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
}
