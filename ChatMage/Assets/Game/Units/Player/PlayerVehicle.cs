using CCC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : Vehicle, IAttackable, IVisible
{
    [Header("Trail Renderers")]
    public string sortingLayer;
    public float beginDriftDelta;
    public bool spawnStdTrails = true;
    public TrailRenderer stdTrailPrefab;
    public bool spawnDriftTrails = true;
    public TrailRenderer driftTrailPrefab;

    private Transform[] trails;

    private bool drifting = false;
    //private List<TrailRenderer> blackTrails = new List<TrailRenderer>();
    [NonSerialized]
    public PlayerController controller;

    public ISpeedOverrider speedOverrider = null;
    public List<ISpeedBuff> speedBuffs = new List<ISpeedBuff>();

    public float RealMoveSpeed()
    {
        return ActualMoveSpeed();
    }
    protected override float ActualMoveSpeed()
    {
        if (speedOverrider != null)
            return speedOverrider.GetSpeed();

        float value = base.ActualMoveSpeed();
        for (int i = 0; i < speedBuffs.Count; i++)
        {
            value += speedBuffs[i].GetAdditionalSpeed();
        }

        return value;
    }

    private StatFloat statMoveSpeed;

    public void Init(PlayerController controller)
    {
        this.controller = controller;
        trails = new Transform[controller.playerLocations.wheels.Length];

        Game.instance.onGameReady += OnGameReady;
    }

    private void OnGameReady()
    {
        if (spawnStdTrails)
        {
            NewTrail(stdTrailPrefab, controller.playerLocations.BackLeftWheel.position);
            NewTrail(stdTrailPrefab, controller.playerLocations.BackRightWheel.position);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (spawnDriftTrails)
        {
            float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(targetDirection, Rotation));
            if (deltaAngle > beginDriftDelta)
            {
                if (!drifting)
                    StartDrift();
            }
            else
            {
                if (drifting)
                    StopDrift();
            }
        }
    }

    public void Kill()
    {
        if (IsDead)
            return;

        Die();
    }

    protected override void Die()
    {
        base.Die();

        //Death animation
        Destroy();
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        amount = CheckBuffs_Attacked(on, amount, otherUnit, source);

        return controller.playerStats.Attacked(on, amount, otherUnit, source);
    }

    public int SmashJuice()
    {
        return 0;
    }

    public bool IsVisible()
    {
        return controller.playerStats.isVisible;
    }

    #region Drift

    private void StartDrift()
    {
        for (int i = 0; i < controller.playerLocations.wheels.Length; i++)
        {
            //Detache l'ancienne trail
            if (trails[i] != null)
                continue;

            //Nouvelle trail
            trails[i] = NewTrail(driftTrailPrefab, controller.playerLocations.wheels[i].position);
        }
        drifting = true;
    }

    private void StopDrift()
    {
        for (int i = 0; i < controller.playerLocations.wheels.Length; i++)
        {
            //Detache l'ancienne trail
            if (trails[i] != null)
            {
                trails[i].SetParent(Game.instance.unitsContainer);
                trails[i] = null;
            }
        }
        drifting = false;
    }

    private Transform NewTrail(TrailRenderer prefab, Vector3 worldPosition)
    {
        TrailRenderer newTrail = Instantiate(prefab.gameObject, worldPosition, Quaternion.identity, controller.body)
            .GetComponent<TrailRenderer>();
        newTrail.sortingLayerName = sortingLayer;
        return newTrail.transform;
    }
    #endregion
}
