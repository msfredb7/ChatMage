using System;

namespace AI
{
    public class ArcherGoal_Reload : Goal<ArcherVehicle>
    {
        public ArcherGoal_Reload(ArcherVehicle veh) : base(veh) { }

        public override void Activate()
        {
            base.Activate();

            veh.animator.ReloadAnimation(veh.GainAmmo, ForceCompletion);
            veh.Stop();
        }

        public override Status Process()
        {
            ActivateIfInactive();
            return status;
        }

        public override void Interrupted()
        {
            base.Interrupted();

            veh.animator.CancelReload();

            ForceFailure();
        }
    }
}
