namespace AI
{
    public class Goal_Idle : Goal<EnemyVehicle>
    {
        protected float duration = -1;

        public Goal_Idle(EnemyVehicle veh) : base(veh)
        {
        }
        public Goal_Idle(EnemyVehicle veh, float duration) : base(veh)
        {
            this.duration = duration;
        }

        public override void Activate()
        {
            base.Activate();

            veh.Stop();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if(duration > 0)
            {
                duration -= veh.DeltaTime();
                if (duration <= 0)
                    status = Status.completed;
            }

            return status;
        }
    }
}
