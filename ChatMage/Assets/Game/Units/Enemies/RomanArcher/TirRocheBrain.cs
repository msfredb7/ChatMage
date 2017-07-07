using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirRocheBrain : EnemyBrain<TirRocheVehicle>
{
    public float attackingMaxRange = 6;
    public float tooCloseRange = 3;

    private float minFleeStay = -1;
    private bool isReloading = false;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.25F);
        Gizmos.DrawSphere(transform.position, attackingMaxRange);
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, tooCloseRange);
    }

    protected override void UpdateWithTarget()
    {
        if (vehicle.IsShooting)
            return;

        float dist = meToTarger.magnitude;
        if (dist > attackingMaxRange && minFleeStay < 0)
        {
            //Get closer or reload!
            vehicle.WalkMode();
            vehicle.useTurnSpeed = true;

            if (vehicle.Ammo == 0 || isReloading)
            {
                //Si on a pas d'ammo OU on est entrain de reload, restont dans cet �tat
                SetBehavior(BehaviorType.Wander);
            }
            else
            {
                //Va vers le joueur
                SetBehavior(BehaviorType.Wander);
            }
        }
        else if (dist > tooCloseRange && minFleeStay < 0)
        {
            //Attack or reload
            vehicle.WalkMode();
            vehicle.useTurnSpeed = true;

            if (vehicle.Ammo == 0 || isReloading)
            {
                //Si on a pas d'ammo OU on est entrain de reload, restont dans cet �tat
                SetBehavior(BehaviorType.Wander);
            }
            else
            {
                //Regarde le joueur et shoot !
                SetBehavior(BehaviorType.Wander);

                if (vehicle.CanShoot && Mathf.Abs(Vector2.Angle(meToTarger, vehicle.WorldDirection2D())) < 4)// < 4 degrée pour aim
                {
                    SetBehavior(BehaviorType.LookPlayer);
                    vehicle.Shoot(target);
                }
            }
        }
        else
        {
            //flee
            if (CurrentBehaviorType != BehaviorType.Flee)
                minFleeStay = 0.5f;

            vehicle.RunMode();
            vehicle.useTurnSpeed = false;
            minFleeStay -= vehicle.DeltaTime();

            SetBehavior(BehaviorType.Flee);
        }
    }

    protected override void UpdateWithoutTarget()
    {
        vehicle.useTurnSpeed = true;
        SetBehavior(BehaviorType.Wander);
    }

    void OnStoppedReloading()
    {
        isReloading = false;
    }

    protected override EnemyBehavior NewWanderBehavior()
    {
        isReloading = true;
        return new TirRocheReloadBehavior(vehicle, OnStoppedReloading, OnStoppedReloading);
    }
}
