using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ShielderGoal_Battle : GoalComposite<ShielderVehicle>
    {
        Unit target;

        public ShielderGoal_Battle(ShielderVehicle veh, Unit target) : base(veh)
        {
            this.target = target;
        }

        public override void Activate()
        {
            base.Activate();

            if (!Unit.HasPresence(target))
            {
                status = Status.failed;
            }
            else
            {
                RemoveAllSubGoals();

                //Ram towards player
                AddSubGoal(new ShielderGoal_RamPlayer(veh, target));


                //Attack goal
                Goal_Tween attackGoal = new Goal_Tween(veh, veh.animator.AttackAnimation());
                attackGoal.stopVehicle = true;
                attackGoal.CanBeInterrupted = false;

                AddSubGoal(attackGoal);
            }

        }

        public override Status Process()
        {
            ActivateIfInactive();

            status = ProcessSubGoals();

            ReactivateIfCompleted();

            return status;
        }
    }
}
