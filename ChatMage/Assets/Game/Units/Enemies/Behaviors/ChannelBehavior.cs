using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChannelBehavior : EnemyBehavior<EnemyVehicle>
{
    //Events
    public Action onComplete;
    public Action<float> onProgress;
    public Action onCancel;

    //Durations
    protected float totalDuration;
    protected float remainingDuration;

    //State
    protected bool completed = false;

    public ChannelBehavior(EnemyVehicle vehicle, float duration)
        : base(vehicle)
    {
        //Durations
        totalDuration = duration;
        remainingDuration = totalDuration;
    }

    public bool Completed { get { return completed; } }
    public float RemainingDuration { get { return remainingDuration; } }
    public float Progress { get { return 1 - (remainingDuration / totalDuration); } }

    /// <summary>
    /// Complete le channel immediatement
    /// </summary>
    public void Rush()
    {
        Complete();
    }

    public override void Enter(Unit target)
    {

    }

    public override void Exit(Unit target)
    {
        //On cancel si incomplete
        if (!completed)
            OnCancel();
    }

    public override void Update(Unit target, float deltaTime)
    {
        //Si pas encore complete
        if (!completed)
        {
            //Decrease timer
            remainingDuration -= deltaTime;

            OnProgress();

            //Completed ?
            if (remainingDuration <= 0)
                Complete();
        }
    }

    private void Complete()
    {
        //Pour ne pas le faire 2 fois
        if (completed)
            return;

        completed = true;
        OnComplete();
    }

    protected virtual void OnCancel()
    {
        if (onCancel != null)
            onCancel();
    }

    protected virtual void OnProgress()
    {
        if (onProgress != null)
            onProgress(Mathf.Clamp01(Progress));
    }

    protected virtual void OnComplete()
    {
        if (onComplete != null)
            onComplete();
    }
}
