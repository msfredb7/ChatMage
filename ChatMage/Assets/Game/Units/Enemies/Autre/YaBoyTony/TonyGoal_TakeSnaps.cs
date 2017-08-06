using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TonyGoal_TakeSnaps : GoalComposite<TonyVehicle>
    {
        private float roamTime;

        public TonyGoal_TakeSnaps(TonyVehicle veh, float roamTime) : base(veh)
        {
            this.roamTime = roamTime;
        }

        public override void Activate()
        {
            base.Activate();

            RemoveAllSubGoals();

            //Trouve un spot a snap
            AddSubGoal(new TonyGoal_FindSpot(veh, (Vector2 zone) =>
            {                
                //Va vers le spot
                Goal_GoTo goalGoTo = new Goal_GoTo(veh, zone);
                goalGoTo.CompleteAfter(3);
                AddSubGoal(goalGoTo);

                //Take snap
                Goal_Tween snapGoal = new Goal_Tween(veh, veh.animator.TakeSnap);
                snapGoal.stopVehicle = true;
                AddSubGoal(snapGoal);

                //Pause
                AddSubGoal(new Goal_Idle(veh, 0.8f));
            }, roamTime));
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
