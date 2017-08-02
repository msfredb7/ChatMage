using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AI
{
    public class BubbleMageGoal_Shoot : BaseGoal_Tween<BubbleMageVehicle>
    {
        private Unit target;
        private TweenCallback shootBubble;

        public BubbleMageGoal_Shoot(BubbleMageVehicle veh, Unit target, TweenCallback shootBubble) : base(veh)
        {
            CanBeInterrupted = false;
            this.target = target;
            this.shootBubble = shootBubble;
        }

        public override void Activate()
        {
            tween = veh.animator.Shoot(shootBubble);

            veh.Stop();

            base.Activate();
        }

        public override Status Process()
        {
            if (IsActive() && Unit.HasPresence(target))
            {
                //Look at target
                Vector2 meToTarget = target.Position - veh.Position;
                veh.TurnToDirection(meToTarget, veh.DeltaTime());
            }

            return base.Process();
        }
    }
}
