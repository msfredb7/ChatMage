using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTweenBehavior<T> : EnemyBehavior<T> where T : EnemyVehicle
{
    protected Tween tween;
    protected bool killTweenOnQuit = true;
    protected bool rewindTweenOnCancel = true;

    public BaseTweenBehavior(T vehicle)
        : base(vehicle)
    {
    }

    public override void Enter(Unit target)
    {
        tween.Play();
        vehicle.onTimeScaleChange += UpdateTimeScale;
        UpdateTimeScale(vehicle);
    }

    protected void SetTween(Tween newTween)
    {
        if (tween != null && tween.IsActive())
            tween.Kill();

        tween = newTween;
        UpdateTimeScale(vehicle);
    }

    protected void UpdateTimeScale(Unit unit)
    {
        tween.timeScale = unit.TimeScale;
    }

    public override void Exit(Unit target)
    {
        if (tween.IsActive() && !tween.IsComplete())
            OnCancel();

        if (killTweenOnQuit)
            tween.Kill();
        tween = null;
        vehicle.onTimeScaleChange -= UpdateTimeScale;
    }

    protected virtual void OnCancel()
    {
        if (rewindTweenOnCancel)
            tween.Rewind(false);
    }

    public float Elapsed { get { return tween.Elapsed(); } }
    public float ElapsedPercentage { get { return tween.ElapsedPercentage(); } }
}
