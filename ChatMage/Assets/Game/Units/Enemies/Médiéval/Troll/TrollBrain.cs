using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TrollBrain : EnemyBrainV2<TrollVehicle>
    {
        public JesusRockV2 myRock;

        private Unit target;
        private bool rockAdded = false;

        Goal attackGoal;

        void Start()
        {
            myRock.PickedUpState(veh);

            AddGoal(new Goal_Wander(veh));
            veh.OnDeath += Veh_onDeath;
        }

        private void Veh_onDeath(Unit unit)
        {
            if (attackGoal != null)
                attackGoal.ForceFailure();
        }

        protected override void Update()
        {
            base.Update();

            CheckRock();

            if(target == null)
            {
                target = veh.targets.TryToFindTarget(veh);

                if(target != null)
                {
                    attackGoal = new TrollGoal_Attack(veh, myRock, target);
                    attackGoal.onRemoved = (Goal g) => { target = null; attackGoal = null; };
                    AddGoal(attackGoal);
                }
            }
        }

        void CheckRock()
        {
            if(!rockAdded && Game.instance != null)
            {
                Game.instance.AddExistingUnit(myRock, false);
                rockAdded = true;
            }
        }
    }

}