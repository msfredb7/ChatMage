using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ArcherShootBehavior : BaseTweenBehavior<ArcherVehicle>
{

    public ArcherShootBehavior(ArcherVehicle vehicle)
        : base(vehicle)
    {
        tween = vehicle.animator.ShootAnim(OnShootMoment);
        tween.OnComplete(OnAnimComplete);
        vehicle.IsShooting = true;

        vehicle.Stop();
    }

    public override void Update(Unit target, float deltaTime)
    {
        //On regarde la cible
        if (target != null)
            vehicle.TurnToDirection(target.Position - vehicle.Position, deltaTime);
    }

    public override void Exit(Unit target)
    {
        base.Exit(target);

        vehicle.IsShooting = false;
    }

    void OnAnimComplete()
    {
        vehicle.IsShooting = false;
    }

    void OnShootMoment()
    {
        ArcherArrow proj = Game.instance.SpawnUnit(vehicle.arrowPrefab, vehicle.arrowLaunchLocation.position);
        
        proj.Init(vehicle, vehicle.WorldDirection2D(), vehicle.targets);

        vehicle.animator.AsNoAmmo();

        vehicle.LoseAmmo();
    }
}
