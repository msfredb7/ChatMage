namespace AI
{
    public class SwordsmanGoal_Attack : BaseGoal_Tween<SwordsmanVehicle>
    {
        private bool lookAtTarget = true;
        private Unit target;

        public SwordsmanGoal_Attack(SwordsmanVehicle myVehicle, Unit target) : base(myVehicle)
        {
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
