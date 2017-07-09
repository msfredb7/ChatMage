using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBrain : EnemyBrain<ArcherVehicle>
{
    public float attackRange = 9;
    public float fleeRange = 3;

    private float minFleeStay = -1;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.25F);
        Gizmos.DrawSphere(transform.position, attackRange);
        Gizmos.color = new Color(1, 0, 0, 0.25F);
        Gizmos.DrawSphere(transform.position, fleeRange);
    }

    protected override void UpdateWithTarget()
    {
        if (vehicle.IsShooting)
            return;

        //S'il reste du temps a fuir, fuyons
        if (minFleeStay > 0)
        {
            minFleeStay -= vehicle.DeltaTime();
            return;
        }

        float dist = meToTarget.magnitude;

        if (dist > attackRange)
        {
            //Get closer or reload!
            vehicle.WalkMode();

            if (vehicle.Ammo == 0)
            {
                //Si on a pas d'ammo OU on est entrain de reload, restont dans cet �tat
                if (CanGoTo<ArcherReloadBehavior>())
                    SetBehavior(new ArcherReloadBehavior(vehicle));
            }
            else
            {
                //Va vers le joueur
                if (CanGoTo<FollowBehavior>())
                    SetBehavior(new FollowBehavior(vehicle));
            }
        }
        else if (dist > fleeRange)
        {
            //Attack or reload
            vehicle.WalkMode();

            if (vehicle.CanShoot)
            {
                float angleDeltaToTarget = Mathf.Abs(Vector2.Angle(meToTarget, vehicle.WorldDirection2D()));
                if (angleDeltaToTarget < 4)
                {
                    //Attaque la cible
                    if (CanGoTo<ArcherShootBehavior>())
                        SetBehavior(new ArcherShootBehavior(vehicle));
                }
                else
                {
                    if (CanGoTo<LookTargetBehavior>())
                        SetBehavior(new LookTargetBehavior(vehicle));
                }
            }
            else if (vehicle.Ammo == 0)
            {
                //Reload
                if (CanGoTo<ArcherReloadBehavior>())
                    SetBehavior(new ArcherReloadBehavior(vehicle));
            }
            else
            {
                //On attend de pouvoir tirer
                if (CanGoTo<ArcherRepositionBehavior>())
                    SetBehavior(new ArcherRepositionBehavior(vehicle));
            }
        }
        else
        {
            //flee
            if (!IsBehavior<FleeBehavior>())
                minFleeStay = 0.5f;

            vehicle.FleeMode();

            if (CanGoTo<FleeBehavior>())
                SetBehavior(new FleeBehavior(vehicle));
        }
    }

    protected override void UpdateWithoutTarget()
    {
        if (CanGoTo<WanderBehavior>())
            SetBehavior(new WanderBehavior(vehicle));
    }
}
