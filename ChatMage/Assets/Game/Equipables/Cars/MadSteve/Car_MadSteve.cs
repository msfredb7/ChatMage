using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class Car_MadSteve : StdCar
{
    public TrailOfColliders trailOfCollidersPrefab;

    [fsIgnore, System.NonSerialized]
    private TrailOfColliders trail;

    protected override void ClearReferences()
    {
        base.ClearReferences();
        trail = null;
    }

    public override void Init(PlayerController player)
    {
        base.Init(player);

        trail = Instantiate(trailOfCollidersPrefab.gameObject).GetComponent<TrailOfColliders>();
        trail.transform.SetParent(Game.instance.transform);
        trail.enabled = false;

        trail.OnTriggerEnter += OnUnitEnterTrail;
    }

    void OnUnitEnterTrail(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit is IAttackable)
        {
            (other.parentUnit as IAttackable).Attacked(other, 1, player.vehicle);
        }
    }

    public override void OnGameStarted()
    {
        base.OnGameStarted();

        trail.SetFollowTarget(player.playerLocations.boule);
        trail.enabled = true;
    }
}
