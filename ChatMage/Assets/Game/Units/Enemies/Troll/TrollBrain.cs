using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TrollBrain : EnemyBrainV2<TrollVehicle>
    {
        public JesusRockV2 myRock;

        private Unit target;

        void Start()
        {
            myRock.PickedUpState(veh);
            Game.instance.AddExistingUnit(myRock);

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
                    Goal attackGoal = new TrollGoal_Attack(veh, myRock, target);
                    attackGoal.onExit = (Goal g) => target = null;
                    AddGoal(attackGoal);
                }
            }
        }
    }

}