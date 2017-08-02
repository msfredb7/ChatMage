using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TrollGoal_Attack : GoalComposite<TrollVehicle>
    {
        JesusRockV2 myRock;
        Unit target;

        public TrollGoal_Attack(TrollVehicle veh, JesusRockV2 myRock, Unit target) : base(veh)
        {
            this.myRock = myRock;
            this.target = target;
        }

        public override void Activate()
        {
            base.Activate();

            if(myRock.InTheHandsOf != veh)
            {
                //Doit-on aller la chercher ?  (cannot fail)
                AddSubGoal(new Goal_Follow(veh, myRock, veh.unitWidth));

                //Aim at target  (cannot fail)
                AddSubGoal(new Goal_LookAt(veh, myRock));

                //Pick up  (cannot fail)
                AddSubGoal(new TrollGoal_RockPickUp(veh, myRock));
            }

            //Aim at target  (CAN FAIL)
            AddSubGoal(new Goal_LookAt(veh, target));

            //Throw  (cannot fail)
            AddSubGoal(new TrollGoal_RockThrow(veh, myRock, target));
        }

        public override Status Process()
        {
            ActivateIfInactive();

            status = ProcessSubGoals();

            return status;
        }
    }

}