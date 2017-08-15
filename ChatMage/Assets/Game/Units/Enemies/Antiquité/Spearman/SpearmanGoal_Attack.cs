namespace AI
{
    public class SpearmanGoal_Attack : Goal<SpearmanVehicle>
    {
        private bool lookAtTarget = true;
        private Unit target;

        public SpearmanGoal_Attack(SpearmanVehicle myVehicle, Unit target) : base(myVehicle)
        {
            CanBeInterrupted = false;
            myVehicle.animator.AttackAnimation(OnAttackMoment, ForceCompletion);
            this.target = target;
        }

        public override void Activate()
        {
            base.Activate();

            veh.Stop();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (lookAtTarget && Unit.HasPresence(target))
            {
                veh.TurnToDirection(target.Position - veh.Position, veh.DeltaTime());
            }

            return status;
        }

        private void OnAttackMoment()
        {
            lookAtTarget = false;
        }
    }
}
