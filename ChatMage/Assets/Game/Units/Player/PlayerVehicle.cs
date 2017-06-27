using CCC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : Vehicle
{
    [Header("Trail Renderers")]
    public float beginDriftDelta;
    public bool spawnStdTrails = true;
    public TrailRenderer stdTrailPrefab;
    public bool spawnDriftTrails = true;
    public TrailRenderer driftTrailPrefab;

    private Transform[] trails;

    private bool drifting = false;
    private List<TrailRenderer> blackTrails = new List<TrailRenderer>();
    PlayerController controller;

    public ISpeedOverrider speedOverrider = null;

    protected override float ActualMoveSpeed()
    {
        if (speedOverrider != null)
            return speedOverrider.GetSpeed();

        return base.ActualMoveSpeed();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopDrift();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartDrift();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();


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
        Destroy(gameObject);
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
        return Instantiate(prefab.gameObject, worldPosition, Quaternion.identity, controller.body).transform;
    }
    #endregion
}
