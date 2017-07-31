using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenBehavior : BaseTweenBehavior<EnemyVehicle>
{
    public Action onComplete;
    public Action onCancel;
    public Action<float> onProgressPercentage;

    public TweenBehavior(EnemyVehicle vehicle, Tween tween, 
        bool killTweenOnQuit = true,
        bool rewindTweenOnQuit = true)
        : base(vehicle)
    {
        this.tween = tween;
        this.killTweenOnQuit = killTweenOnQuit;
        this.rewindTweenOnCancel = rewindTweenOnQuit;
    }

    public override void Enter(Unit target)
    {
        base.Enter(target);

        tween.OnComplete(OnComplete);
    }

    public override void Update(Unit target, float deltaTime)
    {
        if(onProgressPercentage != null)
        {
            onProgressPercentage(tween.ElapsedPercentage());
        }
    }

    void OnComplete()
    {
        if (onComplete != null)
            onComplete();
    }

    protected override void OnCancel()
    {
        base.OnCancel();

        if (onCancel != null)
            onCancel();
    }
}
