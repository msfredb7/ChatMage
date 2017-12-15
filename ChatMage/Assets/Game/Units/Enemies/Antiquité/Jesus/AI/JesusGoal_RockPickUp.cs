using UnityEngine;

namespace AI
{
    public class JesusGoal_RockPickUp : Goal<JesusV2Vehicle>
    {
        private JesusRockV2 myRock;

        public JesusGoal_RockPickUp(JesusV2Vehicle veh, JesusRockV2 myRock) : base(veh)
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
            if(status == Status.active)
            {
                myRock.transform.SetParent(veh.rockTransporter, true);
                myRock.PickedUpState(veh);
            }
        }

        public override void Interrupted()
        {
            base.Interrupted();

            Debug.Log("interrupted");

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