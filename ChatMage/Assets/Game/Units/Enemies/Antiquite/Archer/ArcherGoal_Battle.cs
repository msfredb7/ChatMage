using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Math;
using DG.Tweening;

namespace AI
{
    public class ArcherGoal_Battle : GoalComposite<ArcherVehicle>
    {
        //Repositioning
        private const float MIN_DIST = 1.5f;
        private const float MAX_DIST = 3f;
        private const float TOO_CLOSE_DIST = 10; //CETTE DISTANCE EST ^2

        Unit target;
        private float attackRange;

        public ArcherGoal_Battle(ArcherVehicle veh, Unit target, float attackRange) : base(veh)
        {
            this.target = target;
            this.attackRange = attackRange;
        }

        public override void Activate()
        {
            base.Activate();

            RemoveAllSubGoals();

            //Reposition ?
            if (veh.ShootCooldownRemains > 0)
            {
                Goal_GoTo repositionGoal = new Goal_GoTo(veh, GetRepositionDestination());
                repositionGoal.CompleteAfter(veh.ShootCooldownRemains.Raised(0.75f));
                AddSubGoal(repositionGoal);
            }

            //Reload ?
            if (veh.Ammo == 0)
            {
                ArcherGoal_Reload reloadGoal = new ArcherGoal_Reload(veh);
                AddSubGoal(reloadGoal);
            }

            // Get close to it
            AddSubGoal(new Goal_Follow(veh, target, attackRange));

            //Look at target
            AddSubGoal(new Goal_LookAt(veh, target));

            //Shoot
            ArcherGoal_Shoot shootGoal = new ArcherGoal_Shoot(veh, target)
            {
                CanBeInterrupted = false
            };

            AddSubGoal(shootGoal);
        }

        public override Status Process()
        {
            ActivateIfInactive();

            status = ProcessSubGoals();

            ReactivateIfCompleted();

            return status;
        }

        private Vector2 GetRepositionDestination()
        {
            Vector2 deltaMove = Vector2.zero;
            if (target == null)
            {
                deltaMove = Vectors.RandomVector2(MIN_DIST, MAX_DIST, 0, 360);
            }
            else
            {
                Vector2 meToTarget = target.Position - veh.Position;
                float angleToTarget = meToTarget.ToAngle();

                bool invert = UnityEngine.Random.value > 0.5f;

                if (meToTarget.sqrMagnitude < TOO_CLOSE_DIST)
                {
                    deltaMove = Vectors.RandomVector2(MIN_DIST, MAX_DIST, angleToTarget + 90, angleToTarget + 120);
                }
                else
                {
                    deltaMove = Vectors.RandomVector2(MIN_DIST, MAX_DIST, angleToTarget + 60, angleToTarget + 120);
                }
                if (invert)
                    deltaMove = -deltaMove;

                //But: Donner ceci
                //____________>>>>>_____________________<<<<<__________________
                //____________>>>>>>>>>>___________<<<<<<<<<<__________________
                //____________>>>>>>>>>>>>>>>O<<<<<<<<<<<<<<<__________________
                //____________>>>>>>>>>>___________<<<<<<<<<<__________________
                //____________>>>>>_____________________<<<<<__________________
            }

            var endPos = deltaMove + veh.Position;

            if (Game.Instance != null && Game.Instance.levelScript is LS_EndlessLevel)
            {
                endPos.x = Mathf.Clamp(endPos.x, -8.5f, 8.3f);
            }

            return endPos;
        }
    }
}
