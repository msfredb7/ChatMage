using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace AI
{
    public class ShielderBrain : EnemyBrainV2<ShielderVehicle>
    {
        private Unit target;

        void Start()
        {
            AddGoal(new Goal_Wander(veh));
        }

        protected override void Update()
        {
            base.Update();

            if(target == null)
            {
                target = veh.targets.TryToFindTarget(veh);

                if(target != null)
                {
                    Goal goal = new ShielderGoal_Battle(veh, target);
                    goal.onRemoved = (Goal g) => target = null;
                    AddGoal(goal);
                }
            }

        }
    }
}
