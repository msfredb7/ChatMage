namespace AI
{
    public class Goal_Idle : Goal<EnemyVehicle>
    {
        public Goal_Idle(EnemyVehicle veh) : base(veh)
        {
        }

        public override void Activate()
        {
            base.Activate();

            veh.Stop();
        }

        public override Status Process()
        {
            ActivateIfInactive();
            return status;
        }
    }
}
