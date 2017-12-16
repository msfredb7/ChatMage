using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;
using DG.Tweening;

public class ITM_Chomper : Item, ISpeedOverrider
{
    [InspectorHeader("Linking")]
    public MultipleColliderListener sphereTrigger;

    [InspectorHeader("Settings")]
    public bool debugRays = false;
    public float cooldownOnSameUnit = 1.5f;
    public float globalCooldown = 1;
    public bool resetOnMeleeKill = true;

    [InspectorHeader("Detection Settings")]
    public float boostedMaxRange;
    public float maxRange;
    public float minRange = 0.5f;
    public float maxAttackAngle = 10;

    [InspectorHeader("Dash Settings")]
    public float dashDuration = 0.25f;
    public float turnSpeed = 350;
    public float maxMoveSpeed = 12;
    public AnimationCurve additiveSpeedOverTime;

    //Dynamic references
    [fsIgnore, NonSerialized]
    public MultipleColliderListener listener;
    [fsIgnore, NonSerialized]
    private Coroutine dashCoroutine = null;

    //Dynamic variables
    [fsIgnore, NonSerialized]
    private float currentCooldown = 0;
    [fsIgnore, NonSerialized]
    private bool isDashing = false;
    [fsIgnore, NonSerialized]
    private float oldMoveSpeed;
    [fsIgnore, NonSerialized]
    private bool oldUseWeight;
    [fsIgnore, NonSerialized]
    private float vehicleSpeed;

    public override void OnGameReady()
    {
        listener = Instantiate(sphereTrigger.gameObject, player.body).GetComponent<MultipleColliderListener>();

        listener.GetComponent<CircleCollider2D>().radius = MaxRange;
        player.playerCarTriggers.onUnitKilled += PlayerCarTriggers_onUnitKilled;
    }

    private void PlayerCarTriggers_onUnitKilled(Unit unit, CarSide carTrigger, ColliderInfo other, ColliderListener listener)
    {
        if (resetOnMeleeKill)
            currentCooldown = 0;
    }

    public override void OnGameStarted()
    {
    }

    private float MaxRange { get { return player.playerStats.boostedAOE ? boostedMaxRange : maxRange; } }

    public override void OnUpdate()
    {
        if (debugRays)
        {
            float dir = player.vehicle.Rotation + maxAttackAngle;
            Vector2 vDir = Vehicle.AngleToVector(dir);
            Debug.DrawRay(
                player.vehicle.Position + vDir * minRange, //Start
                vDir * Mathf.Max(MaxRange - minRange, 0),  //Dir
                Color.red);

            dir = player.vehicle.Rotation - maxAttackAngle;
            vDir = Vehicle.AngleToVector(dir);
            Debug.DrawRay(
                player.vehicle.Position + vDir * minRange, //Start
                vDir * Mathf.Max(MaxRange - minRange, 0),  //Dir
                Color.red);
        }

        if (currentCooldown > 0)
            currentCooldown -= player.vehicle.DeltaTime();

        if (currentCooldown <= 0 && listener != null && listener.inContactWith.Count > 0)
        {
            //Verifie l'angle
            for (int i = 0; i < listener.inContactWith.Count; i++)
            {
                Unit unit = listener.inContactWith[i].unit;
                if (!(unit is IAttackable))
                    continue;

                Vector2 deltaPos = unit.Position - player.vehicle.Position;
                if (deltaPos.sqrMagnitude < minRange * minRange)
                    continue;

                float absoluteAngle = Vehicle.VectorToAngle(deltaPos);
                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(player.vehicle.Rotation, absoluteAngle));

                if (deltaAngle < maxAttackAngle)
                {
                    Dash(absoluteAngle);
                    break;
                }
            }
        }
    }

    void Dash(float targetAngle)
    {
        currentCooldown = globalCooldown;

        if (isDashing)
        {
            Game.instance.StopCoroutine(dashCoroutine);
            OnDashEnd();
        }

        //On enregistre les valeur de speed du joueur (pour pouvoir les restorer)
        Vehicle veh = player.vehicle;

        vehicleSpeed = veh.MoveSpeed;
        oldMoveSpeed = veh.MoveSpeed;
        oldUseWeight = veh.useWeight;

        player.playerDriver.enableInput = false;
        player.vehicle.speedOverrider = this;
        veh.useWeight = false;

        //On lance la coroutine
        isDashing = true;
        dashCoroutine = Game.instance.StartCoroutine(DashRoutine(targetAngle, dashDuration));
    }

    IEnumerator DashRoutine(float targetAngle, float duration)
    {
        float remains = duration;
        Vehicle veh = player.vehicle;
        while (remains > 0 && veh != null)
        {
            float deltaTime = veh.DeltaTime();

            //Update movespeed
            vehicleSpeed = oldMoveSpeed + additiveSpeedOverTime.Evaluate(1 - (remains / duration)) * (maxMoveSpeed - oldMoveSpeed);

            //Update rotation
            veh.Rotation = Mathf.MoveTowardsAngle(veh.Rotation, targetAngle, deltaTime * turnSpeed);

            remains -= deltaTime;

            yield return null;
        }
        OnDashEnd();
    }

    private void OnDashEnd()
    {
        isDashing = false;


        if (player != null)
        {
            //On restore les settings de vitesse du joueur
            player.vehicle.useWeight = oldUseWeight;

            player.playerDriver.enableInput = true;
            player.vehicle.speedOverrider = null;
        }
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        listener = null;
    }

    public float GetSpeed()
    {
        return vehicleSpeed;
    }

    public override void Equip(int duplicateIndex)
    {
        throw new NotImplementedException();
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }
}
