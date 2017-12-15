using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class JesusV2AnimatorV2 : EnemyAnimator, IShaker
{
    public JesusV2Vehicle veh;
    public float walkStomp = 0.2f;
    public float deathStomp = 0.2f;
    public float screamShake = 0.2f;

    [Header("SFX")]
    public CCC.Utility.RandomAudioCliptList walkSounds;
    public float walkVolume = 0.15f;

    private Action pickUpMoment;
    private Action pickUpCallback;
    private Action throwMoment;
    private Action throwCallback;
    private Action awakenCallback;
    private Action rightWallCallback;
    private Action leftWallCallback;

    private int pickUpHash = Animator.StringToHash("pickup");
    private int throwHash = Animator.StringToHash("throw");
    private int hasRockHash = Animator.StringToHash("hasRock");
    private int awakenHash = Animator.StringToHash("awaken");

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

    public void Awaken(Action onComplete, Action breakRightWallMoment, Action breakLeftWallMoment)
    {
        rightWallCallback = breakRightWallMoment;
        leftWallCallback = breakLeftWallMoment;

        awakenCallback = onComplete;
        controller.SetTrigger(awakenHash);
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

    public void _AwakenComplete()
    {
        Flush(ref awakenCallback);
    }
    private void _BreakRightWall()
    {
        Flush(ref rightWallCallback);
        _BigStomp();
    }
    private void _BreakLeftWall()
    {
        Flush(ref leftWallCallback);
        _BigStomp();
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
        SoundManager.PlaySFX(walkSounds.Pick(), volume: walkVolume);
        Game.instance.gameCamera.vectorShaker.Shake(walkStomp);
    }
    private void _BigStomp()
    {
        SoundManager.PlaySFX(walkSounds.Pick(), volume: walkVolume + 0.3f);
        Game.instance.gameCamera.vectorShaker.Shake(walkStomp * 2);
    }
}
