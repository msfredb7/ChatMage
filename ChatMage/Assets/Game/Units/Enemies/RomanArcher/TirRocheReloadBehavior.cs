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

    const float CHOOSE_INTERVAL = 2f;
    const float DISTANCE_MIN = 1f;
    const float DISTANCE_MAX = 3f;

    float chooseTimer = 0;
    float lastDirectionPick = 0;

    public override BehaviorType Type
    {
        get
        {
            return BehaviorType.Wander;
        }
    }

    public override void Enter(Unit player)
    {
        if (vehicle.Ammo < vehicle.maxAmmo)
            StartReloadProcess(player);
        else
        {
            if (onFullReload != null)
            {
                onFullReload();
                onFullReload = null;
            }
        }
    }

    void StartReloadProcess(Unit player)
    {
        OnReachSpot();
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

    public override void Exit(Unit player)
    {
        if (onExit != null)
            onExit();
    }

    public override void Update(Unit player, float deltaTime)
    {
        if (inReloadProcess)
        {
            //Wait for it to be over ...
        }
        else
        {
            if (vehicle.Ammo < vehicle.maxAmmo)
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

                    /*
                    Vector2 randomVector = Vehicle.AngleToVector(UnityEngine.Random.Range(0, 360));
                    */

                    Vector2 randomVector = Game.instance.Player.vehicle.Position - vehicle.Position;
                    if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
                        randomVector = Rotate(randomVector, UnityEngine.Random.Range(0,90)).normalized;
                    else
                        randomVector = Rotate(randomVector, UnityEngine.Random.Range(0, -90)).normalized;

                    vehicle.GotoPosition(vehicle.Position + randomVector * UnityEngine.Random.Range(DISTANCE_MIN, DISTANCE_MAX));

                    chooseTimer = CHOOSE_INTERVAL;

                }
            }
        }
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
