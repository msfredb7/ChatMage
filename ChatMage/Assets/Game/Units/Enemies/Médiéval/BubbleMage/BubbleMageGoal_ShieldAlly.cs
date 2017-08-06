using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BubbleMageGoal_ShieldAlly : GoalComposite<BubbleMageVehicle>
    {
        private Unit target;
        private TweenCallback shootBubble;
        private float shootDistance;

        public BubbleMageGoal_ShieldAlly(BubbleMageVehicle veh, Unit target, float shootDistance, TweenCallback shootBubble) : base(veh)
        {
            this.target = target;
            this.shootBubble = shootBubble;
            this.shootDistance = shootDistance;
        }

        public override void Activate()
        {
            base.Activate();

            //Look at ally
            AddSubGoal(new Goal_Follow(veh, target, shootDistance));

            //Look at ally
            AddSubGoal(new Goal_LookAt(veh, target));

            //Charge, while looking at ally
            AddSubGoal(new BubbleMageGoal_Charge(veh, target));

            //Shoot in front of you !
            AddSubGoal(new BubbleMageGoal_Shoot(veh, target, shootBubble));
        }

        public override Status Process()
        {
            ActivateIfInactive();

            status = ProcessSubGoals();

            return status;
        }
    }
}
