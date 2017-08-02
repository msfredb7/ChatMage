namespace AI
{
    public class SpearmanGoal_Attack : BaseGoal_Tween<SpearmanVehicle>
    {
        private bool lookAtTarget = true;
        private Unit target;

        public SpearmanGoal_Attack(SpearmanVehicle myVehicle, Unit target) : base(myVehicle)
        {
            CanBeInterrupted = false;
            tween = myVehicle.animator.AttackAnimation(OnAttackMoment);
            this.target = target;
        }

        public override void Activate()
        {
            base.Activate();

            veh.Stop();
        }

        public override Status Process()
        {
            if (lookAtTarget && Unit.HasPresence(target))
            {
                veh.TurnToDirection(target.Position - veh.Position, veh.DeltaTime());
            }

            return base.Process();
        }

        private void OnAttackMoment()
        {
            lookAtTarget = false;
        }
    }
}
