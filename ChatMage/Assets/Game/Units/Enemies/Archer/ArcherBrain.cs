using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ArcherBrain : EnemyBrainV2<ArcherVehicle>
    {
        public float attackRange = 9;
        public bool fleeEnemy = true;
        public float fleeDistance = 3;

        private float minFleeStay = -1;
        private float fleeDistSQR;
        private Unit target;
        private bool hasFleeGoal = false;   

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 0, 1, 0.25F);
            Gizmos.DrawSphere(transform.position, attackRange);
            Gizmos.color = new Color(1, 0, 0, 0.25F);
            Gizmos.DrawSphere(transform.position, fleeDistance);
        }

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
            fleeDistSQR = fleeDistance * fleeDistance;
        }


        protected override void Update()
        {
            base.Update();

            if(target == null)
            {
                target = veh.targets.TryToFindTarget(veh);
                if(target != null)
                {
                    ArcherGoal_Battle battleGoal = new ArcherGoal_Battle(veh, target, ShootArrow);
                    battleGoal.onRemoved = (Goal g) => target = null;
                    AddGoal(battleGoal);
                }
            }

            if (ShouldFlee())
            {
                LaunchFlee();
            }
        }

        private bool ShouldFlee()
        {
            if (fleeEnemy && target != null && !hasFleeGoal && Unit.HasPresence(target))
            {
                Vector2 meToTarget = target.Position - veh.Position;

                return meToTarget.sqrMagnitude < fleeDistSQR;
            }
            return false; 
        }

        private void LaunchFlee()
        {
            veh.FleeMode();
            hasFleeGoal = true;
            Goal_Flee fleeGoal = new Goal_Flee(veh, target, fleeDistance);
            fleeGoal.onRemoved = (Goal g) =>
            {
                hasFleeGoal = false;
                veh.WalkMode();
            };
            AddForcedGoal(fleeGoal, -5);
        }

        void ShootArrow()
        {
            ArcherArrow proj = Game.instance.SpawnUnit(veh.arrowPrefab, veh.arrowLaunchLocation.position);

            proj.Init(veh, veh.WorldDirection2D(), veh.targets);

            veh.animator.AsNoAmmo();

            veh.OnShoot();
        }
    }
}
