using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using DG.Tweening;

namespace AI
{
    public class SwordsmanBrain : EnemyBrainV2<SwordsmanVehicle>
    {
        [Header("Gourdinier Brain")]
        public float startAttackRange = 2;
        public bool movementPrediction = true;
        [Tooltip("Il va prédire le mouvement du joueur dans x s.")]
        public float thinkAheadLength = 1;

        private Unit target;
        private SwordsmanGoal_Attack attackGoal;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25F);
            Gizmos.DrawSphere(transform.position, startAttackRange);
        }

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
            veh.onArmorLoss += Vehicle_onArmorLoss;
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
                attackGoal = new SwordsmanGoal_Attack(veh, target);
                attackGoal.onExit = (Goal g) => { target = null; attackGoal = null; };
                AddForcedGoal(attackGoal, 99);
            }
        }

        private void Vehicle_onArmorLoss()
        {
            //Cancel l'attaque
            if (attackGoal != null)
                attackGoal.ForceFailure();

            //New Goal
            Goal_Tween goal = new Goal_Tween(veh, veh.animator.LoseArmor());

            //Add forced goal
            AddForcedGoal(goal, 100);
        }
    }

}