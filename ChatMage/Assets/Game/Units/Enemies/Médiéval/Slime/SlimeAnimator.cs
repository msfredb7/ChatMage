using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    public Animator controller;
    public SlimeVehicle vehicle;

    private int jumpHash = Animator.StringToHash("jump");
    private int reproduceHash = Animator.StringToHash("reproduce");
    private int birthHash = Animator.StringToHash("birth");

    private Action reproduceCallback;
    private Action reproduceMoment;

    private Action birthCallback;
    private Action birthSetupCallback;

    void Start()
    {
        controller.SetFloat("invJD", 1 / vehicle.jumpDuration);

        vehicle.onJump += Vehicle_onJump;

        Vehicle_onTimeScaleChange(vehicle);
        vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
    }

    private void Vehicle_onJump()
    {
        controller.SetTrigger(jumpHash);
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        controller.speed = unit.TimeScale;
    }

    private void _ReproduceComplete()
    {
        if (reproduceCallback != null)
            reproduceCallback();
    }
    private void _ReproduceMoment()
    {
        if (reproduceMoment != null)
            reproduceMoment();
    }

    public void ReproduceAnimation(Action reproduceMoment, Action onComplete)
    {
        reproduceCallback = onComplete;
        this.reproduceMoment = reproduceMoment;
        controller.SetTrigger(reproduceHash);
    }

    private void _BirthComplete()
    {
        if (birthCallback != null)
            birthCallback();
    }
    private void _BirthSetup()
    {
        if (birthSetupCallback != null)
            birthSetupCallback();
    }

    public void BirthAnimation(Action setupMoment, Action onComplete)
    {
        controller.SetTrigger(birthHash);
        birthCallback = onComplete;
        birthSetupCallback = setupMoment;
    }
}
