using UnityEngine;

namespace AI
{
    public class JesusGoal_RockThrow : Goal<JesusV2Vehicle>
    {
        private JesusRockV2 myRock;
        private Unit target;
        private bool lookAtTarget = true;

        public JesusGoal_RockThrow(JesusV2Vehicle veh, JesusRockV2 myRock, Unit target) : base(veh)
        {
            this.myRock = myRock;
            this.target = target;
        }

        public override void Activate()
        {
            veh.animator.ThrowRock(ThrowMoment, ForceCompletion);
            veh.Stop();
            base.Activate();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            //Look at target
            if (lookAtTarget && IsActive() && Unit.HasPresence(target))
            {
                veh.TurnToDirection(target.Position - veh.Position, veh.DeltaTime());
            }

            return status;
        }

        void ThrowMoment()
        {
            lookAtTarget = false;
            myRock.flySpeed = veh.throwSpeed * (veh.TimeScale + 1) / 2;
            myRock.ThrownState(veh.WorldDirection2D(), veh);
            myRock.transform.SetParent(Game.Instance.unitsContainer, true);
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
                myRock.transform.SetParent(Game.Instance.unitsContainer, true);
                myRock.StoppedState();
            }

            base.ForceFailure();
        }
    }
}