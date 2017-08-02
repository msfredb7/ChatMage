using UnityEngine;

namespace AI
{
    public class TrollGoal_RockPickUp : BaseGoal_Tween<TrollVehicle>
    {
        private JesusRockV2 myRock;

        public TrollGoal_RockPickUp(TrollVehicle veh, JesusRockV2 myRock) : base(veh)
        {
            this.myRock = myRock;
        }

        public override void Activate()
        {
            tween = veh.animator.PickUpRockAnimation(PickUpMoment);
            veh.Stop();
            base.Activate();
        }

        void PickUpMoment()
        {
            myRock.transform.SetParent(veh.rockTransporter, true);
            myRock.PickedUpState(veh);
        }

        public override void LoseFocus()
        {
            base.LoseFocus();

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
    }
}