using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BardBrain : EnemyBrainV2<BardVehicle>
    {
        public float singEvery = 6;
        public bool fleeEnemy = true;
        public float fleeDistance = 3;

        private float singTimer;
        private Unit target;
        private bool hasFleeGoal = false;
        private float fleeDistSQR;

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
            singTimer = Random.Range(singEvery * 0.35f, singEvery);
            fleeDistSQR = fleeDistance * fleeDistance;
        }

        protected override void Update()
        {
            base.Update();

            if (!hasFleeGoal)
            {
                if (target == null)
                    target = veh.targets.TryToFindTarget(veh);

                if (Unit.HasPresence(target))
                {
                    Vector2 meToTarget = target.Position - veh.Position;
                    if (meToTarget.sqrMagnitude < fleeDistSQR)
                    {
                        hasFleeGoal = true;

                        Goal fleeGoal = new Goal_Flee(veh, target, fleeDistance);
                        fleeGoal.onRemoved = (Goal g) => hasFleeGoal = false;
                        AddForcedGoal(fleeGoal, -5);
                    }
                }
            }

            if(singTimer <= 0)
            {
                singTimer = float.PositiveInfinity;
                Goal singGoal = new BardGoal_Sing(veh);
                singGoal.onRemoved = (Goal g) => ResetSingTimer();
                AddGoal(singGoal);
            }

            singTimer -= veh.DeltaTime();
        }

        private void ResetSingTimer()
        {
            singTimer = singEvery;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 0, 1, 0.2f);
            Gizmos.DrawSphere(transform.position, fleeDistance);
        }
    }
}
