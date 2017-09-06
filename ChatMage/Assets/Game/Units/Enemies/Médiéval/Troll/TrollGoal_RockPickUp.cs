using System;
using UnityEngine;

namespace AI
{
    public class TrollGoal_RockPickUp : Goal<TrollVehicle>
    {
        private JesusRockV2 myRock;

        public TrollGoal_RockPickUp(TrollVehicle veh, JesusRockV2 myRock) : base(veh)
        {
            this.myRock = myRock;
        }

        public override void Activate()
        {
            veh.animator.PickUpRock(PickUpMoment, ForceCompletion);
            veh.Stop();
            base.Activate();
        }

        void PickUpMoment()
        {
            myRock.transform.SetParent(veh.rockTransporter, true);
            myRock.PickedUpState(veh);
        }

        public override void Interrupted()
        {
            base.Interrupted();

            ForceFailure();
        }

        public override void ForceFailure()
        {
            if (myRock.InTheHandsOf == veh)
            {
                myRock.transform.SetParent(Game.instance.unitsContainer, true);
                myRock.StoppedState();
            }

            base.ForceFailure();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            return status;
        }
    }
}