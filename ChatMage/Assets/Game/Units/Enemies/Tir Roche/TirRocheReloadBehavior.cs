using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheReloadBehavior : EnemyBehavior<TirRocheVehicle>
{
    const float PICK_DISTANCE_MAX = 2.5f;
    const float PICK_DISTANCE_MIN = 0.75f;

    public TirRocheReloadBehavior(TirRocheVehicle v, Action onExit, Action onFullReload) : base(v) { this.onFullReload = onFullReload; this.onExit = onExit; }

    private Action onFullReload;
    private Action onExit;
    private bool inReloadProcess = false;

    const float CHOOSE_INTERVAL = 4f;
    const float DISTANCE_MIN = 0.75f;
    const float DISTANCE_MAX = 3.5f;

    float chooseTimer = 0;
    float lastDirectionPick = 0;

    public override BehaviorType Type
    {
        get
        {
            return BehaviorType.Wander;
        }
    }

    public override void Enter(PlayerController player)
    {
        if (vehicle.Ammo < vehicle.maxAmmo)
            StartReloadProcess(player);
    }

    void StartReloadProcess(PlayerController player)
    {
        float chosenAngle = 0;
        if (player != null)
        {
            float angleToPlayer = Vehicle.VectorToAngle(player.vehicle.Position - vehicle.Position);
            chosenAngle = UnityEngine.Random.Range(angleToPlayer + 60, angleToPlayer + 300);
        }
        else
        {
            chosenAngle = UnityEngine.Random.Range(0, 360);
        }

        Vector2 pickedDelta = Vehicle.AngleToVector(chosenAngle) * UnityEngine.Random.Range(PICK_DISTANCE_MIN, PICK_DISTANCE_MAX);
        
        vehicle.GotoPosition(pickedDelta + vehicle.Position, OnReachSpot);
        inReloadProcess = true;
    }

    void OnReachSpot()
    {
        vehicle.Reload(OnReloaded);
    }

    void OnReloaded()
    {
        inReloadProcess = false;
        if (vehicle.Ammo == vehicle.maxAmmo)
        {
            if (onFullReload != null)
            {
                onFullReload();
                onFullReload = null;
            }
        }
    }

    public override void Exit(PlayerController player)
    {
        if (onExit != null)
            onExit();
    }

    public override void Update(PlayerController player, float deltaTime)
    {
        if (inReloadProcess)
        {
            //Wait for it to be over ...
        }
        else
        {
            if(vehicle.Ammo < vehicle.maxAmmo)
            {
                StartReloadProcess(player);
            }
            else
            {
                //STANDARD WANDER

                chooseTimer -= deltaTime;

                if (chooseTimer < 0)
                {
                    //Pick new destination
                    if (lastDirectionPick > 360)
                        lastDirectionPick -= 360;

                    Vector2 randomVector = Vehicle.AngleToVector(UnityEngine.Random.Range(0, 360));

                    vehicle.GotoPosition(vehicle.Position + randomVector * UnityEngine.Random.Range(DISTANCE_MIN, DISTANCE_MAX));

                    chooseTimer = CHOOSE_INTERVAL;
                }
            }
        }
    }
}
