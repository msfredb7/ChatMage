using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BardSingBehavior : BaseTweenBehavior<BardVehicle>, IShaker
{
    private Action onComplete;

    public BardSingBehavior(BardVehicle v, TweenCallback onComplete) : base(v)
    {
        SetTween(vehicle.animator.SingAnimation(OnSingBegin, OnSingEnd).OnComplete(onComplete));
    }

    public override void Enter(Unit target)
    {
        vehicle.Stop();
    }

    public override void Exit(Unit target)
    {
        Game.instance.gameCamera.vectorShaker.RemoveShaker(this);
    }

    public float GetShakeStrength()
    {
        return 0.1f;
    }

    public override void Update(Unit target, float deltaTime)
    {

    }

    private void OnSingBegin()
    {
        Game.instance.gameCamera.vectorShaker.AddShaker(this);

        Allegiance[] allowAlligience = new Allegiance[]
        {
            Allegiance.Enemy
        };
        List<Unit> hitUnits = UnitDetection.OverlapCircleUnits(vehicle.Position, vehicle.singRadius, vehicle, allowAlligience);

        for (int i = 0; i < hitUnits.Count; i++)
        {
            float cap = hitUnits[i] is BardVehicle ? vehicle.capOnOtherBards : vehicle.defaultCap;

            float value = vehicle.timeScaleMultiplier;
            float timescale = hitUnits[i].TimeScale;

            value = Mathf.Min(1, Mathf.Max(cap / timescale, value));

            if (value > 1.001f)
                hitUnits[i].AddBuff(new TimeScaleBuff(value, vehicle.duration, true));
        }
    }

    private void OnSingEnd()
    {
        Game.instance.gameCamera.vectorShaker.RemoveShaker(this);
    }
}
