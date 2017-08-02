using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ShielderGoal_RamPlayer : Goal<ShielderVehicle>
    {
        Unit target;
        public ShielderGoal_RamPlayer(ShielderVehicle veh, Unit target) : base(veh)
        {
            this.target = target;
        }

        public override void Activate()
        {
            base.Activate();

            veh.onShieldPhysicalHit = delegate (Unit unit)
            {
                if (veh.targets.IsValidTarget(unit))
                {
                    status = Status.completed;
                }
            };
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (IsActive())
            {
                if (Unit.HasPresence(target))
                {
                    veh.GotoPosition(target.Position);
                }
                else
                {
                    status = Status.failed;
                }
            }


            return status;
        }

        public override void Removed()
        {
            base.Removed();

            veh.onShieldPhysicalHit = null;
        }
    }

}