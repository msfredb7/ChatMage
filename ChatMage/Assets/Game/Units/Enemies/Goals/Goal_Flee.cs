namespace AI
{
    public class Goal_Flee : Goal<EnemyVehicle>
    {
        private Unit target;

        public Goal_Flee(EnemyVehicle myVehicle, Unit target) : base(myVehicle)
        {
            this.target = target;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (Unit.HasPresence(target))
            {
                veh.GotoDirection(veh.Position - target.Position, veh.DeltaTime());
            }
            else
            {
                status = Status.completed;
            }

            return status;
        }
    }
}