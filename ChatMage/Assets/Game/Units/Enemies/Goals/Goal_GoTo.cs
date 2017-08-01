using UnityEngine;

namespace AI
{
    public class Goal_GoTo : Goal<EnemyVehicle>
    {
        private Vector2 destination;
        private bool reached = false;

        public Goal_GoTo(EnemyVehicle veh, Vector2 position) : base(veh)
        {
            destination = position;
        }

        public override void Activate()
        {
            base.Activate();

            veh.GotoPosition(destination, () => reached = true);
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (reached)
            {
                status = Status.completed;
            }

            return status;
        }
    }
}
