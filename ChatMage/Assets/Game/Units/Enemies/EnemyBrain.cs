using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;


public enum EnemyMoveState { Custom = 0, Wander = 1, Panic = 2, Flee = 3, FollowPlayer = 4, Idle = 5, LookAtPlayer = 6 }

/// <summary>
/// Enemy Brain, with vehicle type T
/// </summary>
/// <typeparam name="T">Vehicle Type</typeparam>
public abstract class EnemyBrain<T> : BaseBehavior where T : EnemyVehicle
{
    [InspectorHeader("Enemy Brain")]
    public EnemyMoveState moveState;

    protected T vehicle;
    protected PlayerController player;
    protected Vector2 meToPlayer = Vector2.zero;

    private const float PanicInterval = 1.5f;
    private const float WanderInterval = 4;
    private const float WanderDistanceMax = 3.5f;
    private const float WanderDistanceMin = 0.75f;

    private bool noPlayerOnThisFrame = false;
    private float panicOrWanderRemains = -1;
    private float lastPanicPick = 0;

    protected virtual void Start()
    {
        vehicle = GetComponent<T>();
        if (vehicle == null)
            Debug.LogError("Could not find vehicle of type: " + typeof(T) + ".");
        player = Game.instance.Player;
        vehicle.Stop();
    }

    void Update()
    {
        noPlayerOnThisFrame = player == null || !player.playerStats.isVisible || player.playerStats.isDead || !player.gameObject.activeSelf;

        if (!noPlayerOnThisFrame)
            meToPlayer = player.vehicle.Position - vehicle.Position;

        if (noPlayerOnThisFrame)
            UpdateNoPlayer();
        else
            UpdatePlayer();


        switch (moveState)
        {
            default:
            case EnemyMoveState.Custom:
                break;
            case EnemyMoveState.Wander:
                WanderUpdate();
                break;
            case EnemyMoveState.Panic:
                PanicUpdate();
                break;
            case EnemyMoveState.Flee:
                FleeUpdate();
                break;
            case EnemyMoveState.FollowPlayer:
                FollowUpdate();
                break;
            case EnemyMoveState.Idle:
                IdleUpdate();
                break;
            case EnemyMoveState.LookAtPlayer:
                LookAtPlayerUpdate();
                break;
        }
    }

    protected abstract void UpdatePlayer();
    protected abstract void UpdateNoPlayer();

    void WanderUpdate()
    {
        panicOrWanderRemains -= vehicle.DeltaTime();

        if (panicOrWanderRemains < 0)
        {
            //Pick new destination
            if (lastPanicPick > 360)
                lastPanicPick -= 360;

            Vector2 randomVector = Vehicle.AngleToVector(Random.Range(0, 360));

            vehicle.GotoPosition(vehicle.Position + randomVector * Random.Range(WanderDistanceMin, WanderDistanceMax));

            panicOrWanderRemains = WanderInterval;
        }
    }

    void PanicUpdate()
    {
        panicOrWanderRemains -= vehicle.DeltaTime();

        if (panicOrWanderRemains < 0)
        {
            //Pick new destination
            if (lastPanicPick > 360)
                lastPanicPick -= 360;

            //On met cette petite formule pour ne pas pick 2 fois des direction semblable
            vehicle.GotoDirection(lastPanicPick = Random.Range(lastPanicPick + 60, lastPanicPick + 300));
            panicOrWanderRemains = PanicInterval;
        }
    }

    void FleeUpdate()
    {
        if (noPlayerOnThisFrame)
            return;

        vehicle.GotoDirection(-meToPlayer);
    }

    void FollowUpdate()
    {
        if (noPlayerOnThisFrame)
            return;

        vehicle.GotoDirection(meToPlayer);
    }

    void IdleUpdate()
    {
        vehicle.Stop();
    }

    void LookAtPlayerUpdate()
    {
        if (noPlayerOnThisFrame)
            return;

        vehicle.Stop();
        vehicle.TurnToDirection(meToPlayer);
    }
}
