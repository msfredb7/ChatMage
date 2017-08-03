using UnityEngine;

namespace AI
{
    public class JesusGoal_RockThrow : BaseGoal_Tween<JesusV2Vehicle>
    {
        private JesusRockV2 myRock;
        private Unit target;

        public JesusGoal_RockThrow(JesusV2Vehicle veh, JesusRockV2 myRock, Unit target) : base(veh)
        {
            this.myRock = myRock;
            this.target = target;
        }

        public override void Activate()
        {
            tween = veh.animator.ThrowRockAnimation(ThrowMoment);
            veh.Stop();
            base.Activate();
        }

        public override Status Process()
        {
            //Look at target
            if (IsActive() && Unit.HasPresence(target))
            {
                veh.TurnToDirection(target.Position - veh.Position, veh.DeltaTime());
            }

            return base.Process();
        }

        void ThrowMoment()
        {
            myRock.flySpeed = veh.throwSpeed * (veh.TimeScale + 1) / 2;
            myRock.ThrownState(veh.WorldDirection2D(), veh);
            myRock.transform.SetParent(Game.instance.unitsContainer, true);
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
    }
}