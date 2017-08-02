using UnityEngine;
using FullInspector;

namespace AI
{
    public class SpearmanBrain : EnemyBrainV2<SpearmanVehicle>
    {
        [Header("Gourdinier Brain")]
        public float startAttackRange = 2;
        public bool movementPrediction = true;
        [Tooltip("Il va prédire le mouvement du joueur dans x s.")]
        public float thinkAheadLength = 1;

        private Unit target;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25F);
            Gizmos.DrawSphere(transform.position, startAttackRange);
        }

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
        }

        protected override void Update()
        {
            base.Update();

            if (target == null)
            {
                target = veh.targets.TryToFindTarget(veh);

                if (target != null)
                {
                    Goal_Follow goalFollow;
                    if (movementPrediction)
                        goalFollow = new Goal_Follow(veh, target, startAttackRange, thinkAheadLength);
                    else
                        goalFollow = new Goal_Follow(veh, target, startAttackRange);

                    goalFollow.onExit = OnFollowExit;

                    AddGoal(goalFollow);
                }
            }
        }

        void OnFollowExit(Goal pastGoal)
        {
            if (pastGoal.HasFailed())
            {
                target = null;
            }
            else
            {
                //Add attack goal
                Goal attackGoal = new SpearmanGoal_Attack(veh, target);
                attackGoal.onExit = (Goal g) => target = null;
                AddForcedGoal(attackGoal, 100);
            }
        }
    }

}