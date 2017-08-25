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

        private Unit friendTarget;
        private Unit enemyTarget;
        private bool isFleeing = false;

        void Start()
        {
            AddGoal(new Goal_Wander(veh));

            fleeDistSQR = fleeDistance * fleeDistance;
        }

        protected override void Update()
        {
            base.Update();

            shootCooldownRemains -= veh.DeltaTime();

            if (friendTarget == null && shootCooldownRemains <= 0)
            {
                friendTarget = SearchForUnBubbledEnemy();

                if (friendTarget != null)
                {
                    Goal shieldGoal = new BubbleMageGoal_ShieldAlly(veh, friendTarget, shootDistance, ShootBubble);
                    shieldGoal.onRemoved = (Goal g) => friendTarget = null;
                    AddGoal(shieldGoal);
                }
            }

            //Check for enemy targets
            if (enemyTarget == null)
                enemyTarget = veh.targets.TryToFindTarget(veh);

            //Check if enemy is too close
            if(enemyTarget != null && !isFleeing)
            {
                Vector2 meToTarget = enemyTarget.Position - veh.Position;
                if(meToTarget.sqrMagnitude < fleeDistSQR)
                {
                    //Flee !
                    isFleeing = true;
                    Goal fleeGoal = new Goal_Flee(veh, enemyTarget, fleeDistance);
                    fleeGoal.onRemoved = (Goal g) => isFleeing = false;
                    AddForcedGoal(fleeGoal, -5);
                }
            }
        }

        private void ShootBubble()
        {
            shootCooldownRemains = shootCooldown;
            Game.instance.SpawnUnit(projectilePrefab, shootEmitter.position)
                .Init(veh.Rotation, veh, friendTarget != null ? friendTarget.transform : null);

            shootCooldownRemains = shootCooldown;
        }

        private Unit SearchForUnBubbledEnemy()
        {
            if (Game.instance == null)
                return null;

            Unit recordHolder = null;
            float record = float.PositiveInfinity;

            Vector2 myPos = veh.Position;
            foreach (Unit unit in Game.instance.attackableUnits)
            {
                if (unit == veh)
                    continue;
                if (!unit.IsDead && bubbleTargets.IsValidTarget(unit) && !unit.HasBuffOfType(typeof(BubbleBuff)))
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
