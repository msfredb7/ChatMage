using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BubbleMageGoal_Shoot : Goal<BubbleMageVehicle>
    {
        private Unit target;
        private Action shootBubble;
        private bool charging;

        public BubbleMageGoal_Shoot(BubbleMageVehicle veh, Unit target, Action shootBubble) : base(veh)
        {
            CanBeInterrupted = false;
            this.target = target;
            this.shootBubble = shootBubble;
        }

        public override void Activate()
        {
            charging = true;
            veh.animator.AttackAnimation(
                ()=>
                {
                    charging = false;
                }, 
                shootBubble,
                ForceCompletion);

            veh.Stop();

            base.Activate();
        }

        public override void Interrupted()
        {
            base.Interrupted();

            veh.animator.CancelAttack();
        }

        public override void ForceFailure()
        {
            base.ForceFailure();

            veh.animator.CancelAttack();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (IsActive())
            {
                if (Unit.HasPresence(target))
                {
                    //Look at target
                    Vector2 meToTarget = target.Position - veh.Position;
                    veh.TurnToDirection(meToTarget, veh.DeltaTime());
                }
                else if (charging)
                {
                    //La target n'est plus. on fail
                    ForceFailure();
                }
            }

            return status;
        }
    }
}
