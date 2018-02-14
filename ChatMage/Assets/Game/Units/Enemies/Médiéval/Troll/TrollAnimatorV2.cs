using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAnimatorV2 : EnemyAnimator
{
    public TrollVehicle veh;
    public float stompShake = 0.2f;

    public AudioPlayable stompSound;
    public AudioPlayable bigStompSound;

    private Action pickUpMoment;
    private Action pickUpCallback;
    private Action throwMoment;
    private Action throwCallback;

    private int pickUpHash = Animator.StringToHash("pickup");
    private int throwHash = Animator.StringToHash("throw");
    private int hasRockHash = Animator.StringToHash("hasRock");
    private int deathDirHash = Animator.StringToHash("deathDir");

    protected override EnemyVehicle Vehicle
    {
        get { return veh; }
    }

    public void PickUpRock(Action pickUpMoment, Action onComplete)
    {
        this.pickUpMoment = pickUpMoment;
        pickUpCallback = onComplete;
        controller.SetTrigger(pickUpHash);
    }

    public void ThrowRock(Action throwMoment, Action onComplete)
    {
        this.throwMoment = throwMoment;
        throwCallback = onComplete;

        controller.SetTrigger(throwHash);
    }

    /// <summary>
    /// 0 = front, 1 = left, 2 = right, 3 = back
    /// </summary>
    public void SetDeathDir(int dir)
    {
        controller.SetInteger(deathDirHash, dir);
    }

    public void _PickUpComplete()
    {
        Flush(ref pickUpCallback);
    }
    private void _PickUpMoment()
    {
        Flush(ref pickUpMoment);
        controller.SetBool(hasRockHash, true);
    }
    public void _ThrowComplete()
    {
        Flush(ref throwCallback);
    }
    private void _ThrowMoment()
    {
        Flush(ref throwMoment);
        controller.SetBool(hasRockHash, false);
    }
    private void _Stomp()
    {
        DefaultAudioSources.PlaySFX(stompSound);
        Game.Instance.gameCamera.vectorShaker.Shake(stompShake);
    }
    private void _BigStomp()
    {
        DefaultAudioSources.PlaySFX(stompSound);
        Game.Instance.gameCamera.vectorShaker.Shake(stompShake * 2);
    }
}
