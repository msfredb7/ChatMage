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
        trail.transform.SetParent(Game.Instance.transform);
        trail.enabled = false;

        trail.OnTriggerEnter += OnUnitEnterTrail;
    }

    void OnUnitEnterTrail(ColliderInfo other, ColliderListener listener)
    {
        Unit unit = other.parentUnit;
        if (unit is IAttackable)
        {
            bool wasDead = unit.IsDead;

            IAttackable attackable = unit as IAttackable;
            attackable.Attacked(other, 1, player.vehicle);

            if (unit.IsDead && !wasDead)
                player.playerStats.RegisterKilledUnit(unit);
        }
    }

    public override void OnGameStarted()
    {
        base.OnGameStarted();

        trail.SetFollowTarget(player.playerLocations.boule);
        trail.enabled = true;
    }
}
