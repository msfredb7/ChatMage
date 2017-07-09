using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ArcherReloadBehavior : BaseTweenBehavior<ArcherVehicle>
{
    public ArcherReloadBehavior(ArcherVehicle vehicle)
        : base(vehicle)
    {
        tween = vehicle.animator.ReloadAnimation().OnComplete(OnComplete);
    }

    public override void Enter(Unit target)
    {
        base.Enter(target);

        vehicle.EngineOff();
    }

    public override void Update(Unit target, float deltaTime)
    {

    }

    private void OnComplete()
    {
        vehicle.GainAmmo();
    }
}
