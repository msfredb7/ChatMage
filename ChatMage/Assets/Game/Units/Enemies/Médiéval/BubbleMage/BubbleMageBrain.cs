using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BubbleMageBrain : EnemyBrainV2<BubbleMageVehicle>
    {
        public Targets bubbleTargets;
        public bool fleePlayer = false;
        public float fleeDistance = 3;
        public float shootDistance = 4;
        [Header("Devrais etre plus long que la duree de voyage de la bubble")]
        public float shootCooldown = 2;
        public BubbleProjectile projectilePrefab;
        public Transform shootEmitter;

        private float fleeDistSQR;
        private float shootCooldownRemains = 0;

        private Unit shootTarget;
        private Unit fleeTarget;
        private bool isFleeing = false;
        private Vector2 lastKnownTargetPosition;

        void Start()
        {
            AddGoal(new Goal_Wander(veh));

            fleeDistSQR = fleeDistance * fleeDistance;
        }

        protected override void Update()
        {
            base.Update();

            shootCooldownRemains -= veh.DeltaTime();

            if (shootTarget == null && shootCooldownRemains <= 0)
            {
                shootTarget = SearchForShootingTarget();

                if (shootTarget != null)
                {
                    Goal shieldGoal = new BubbleMageGoal_ShieldAlly(veh, shootTarget, shootDistance, ShootBubble)
                    {
                        onRemoved = (Goal g) => shootTarget = null
                    };
                    AddGoal(shieldGoal);
                }
            }

            //Check for enemy targets
            if (fleeTarget == null)
                fleeTarget = veh.targets.TryToFindTarget(veh);

            //Check if enemy is too close
            if(fleeTarget != null && !isFleeing)
            {
                Vector2 meToTarget = fleeTarget.Position - veh.Position;
                if(meToTarget.sqrMagnitude < fleeDistSQR)
                {
                    //Flee !
                    isFleeing = true;
                    Goal fleeGoal = new Goal_Flee(veh, fleeTarget, fleeDistance)
                    {
                        onRemoved = (Goal g) => isFleeing = false
                    };
                    AddForcedGoal(fleeGoal, -5);
                }
            }

            if (shootTarget != null)
                lastKnownTargetPosition = shootTarget.Position;
        }

        private void ShootBubble()
        {
            ExplosifyMage explosifyMage = GetComponent<ExplosifyMage>();

            if(explosifyMage != null)
            {
                // Explosion
                if (shootTarget != null)
                    explosifyMage.ShootAtTarget(shootTarget, shootEmitter.position);
                else
                    explosifyMage.ShootAtPosition(lastKnownTargetPosition, shootEmitter.position);
            }
            else
            {
                // Bubble
                Game.Instance.SpawnUnit(projectilePrefab, shootEmitter.position)
                    .Init(veh.Rotation, veh, shootTarget);
            }

            shootCooldownRemains = shootCooldown;
        }

        private Unit SearchForShootingTarget()
        {
            if (Game.Instance == null)
                return null;

            ExplosifyMage explosifyMage = GetComponent<ExplosifyMage>();
            Unit recordHolder = null;
            float record = float.PositiveInfinity;

            Vector2 myPos = veh.Position;
            foreach (Unit unit in Game.Instance.attackableUnits)
            {
                if (unit == veh)
                    continue;
                if (!unit.IsDead && bubbleTargets.IsValidTarget(unit) && (!unit.HasBuffOfType(typeof(BubbleBuff)) || explosifyMage))
                {
                    Vector2 v = unit.Position - myPos;
                    float dist = v.sqrMagnitude;
                    if (dist < record)
                    {
                        record = dist;
                        recordHolder = unit;
                    }
                }
            }
            return recordHolder;
        }
    }
}
