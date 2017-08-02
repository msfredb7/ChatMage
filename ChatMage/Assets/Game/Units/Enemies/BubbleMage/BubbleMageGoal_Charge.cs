using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BubbleMageGoal_Charge : BaseGoal_Tween<BubbleMageVehicle>
    {
        private Unit target;

        public BubbleMageGoal_Charge(BubbleMageVehicle veh, Unit target) : base(veh)
        {
            this.target = target;
        }

        public override void Activate()
        {
            tween = veh.animator.Charge();

            veh.Stop();

            base.Activate();
        }

        public override Status Process()
        {
            if (IsActive())
            {
                if (Unit.HasPresence(target))
                {
                    //Look at target
                    Vector2 meToTarget = target.Position - veh.Position;
                    veh.TurnToDirection(meToTarget, veh.DeltaTime());
                }
                else
                {
                    //La target n'est plus. on fail
                    ForceFailure();
                }
            }

            return base.Process();
        }

        public override void Interrupted()
        {
            base.Interrupted();

            ForceFailure();
        }
    }
}
