using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusV2AnimatorV2 : EnemyAnimator, IShaker
{
    public JesusV2Vehicle veh;
    public float walkStomp = 0.2f;
    public float deathStomp = 0.2f;
    public float screamShake = 0.2f;

    private Action pickUpMoment;
    private Action pickUpCallback;
    private Action throwMoment;
    private Action throwCallback;

    private int pickUpHash = Animator.StringToHash("pickup");
    private int throwHash = Animator.StringToHash("throw");
    private int hasRockHash = Animator.StringToHash("hasRock");

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

    private void _StartScream()
    {
        Game.instance.gameCamera.vectorShaker.AddShaker(this);
    }
    private void _EndScream()
    {
        Game.instance.gameCamera.vectorShaker.RemoveShaker(this);
    }
    public float GetShakeStrength()
    {
        return screamShake;
    }

    private void _Stomp()
    {
        Game.instance.gameCamera.vectorShaker.Shake(walkStomp);
    }
    private void _BigStomp()
    {
        Game.instance.gameCamera.vectorShaker.Shake(walkStomp * 2);
    }
}
