using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class Car_WarpStar : StdCar
{
    public float timescaleMultiplier;
    public TrailOfColliders trailofCollidersPrefab;

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

        trail = Instantiate(trailofCollidersPrefab.gameObject).GetComponent<TrailOfColliders>();
        trail.transform.SetParent(Game.Instance.transform);

        trail.enabled = false;

        trail.OnTriggerEnter += OnUnitEnterTrail;
        trail.OnTriggerExit += OnUnitExitTrail;
    }

    void OnUnitEnterTrail(ColliderInfo other, ColliderListener listener)
    {
        other.parentUnit.TimeScale *= timescaleMultiplier;
    }

    void OnUnitExitTrail(ColliderInfo other, ColliderListener listener)
    {
        other.parentUnit.TimeScale /= timescaleMultiplier;
    }

    public override void OnGameStarted()
    {
        base.OnGameStarted();

        trail.SetFollowTarget(player.playerLocations.boule);
        trail.enabled = true;
    }
}
