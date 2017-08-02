using UnityEngine;

namespace AI
{
    public class Goal_GoTo : Goal<EnemyVehicle>
    {
        private Vector2 destination;
        private bool reached = false;

        private bool failAfter;
        private bool completeAfter;
        private float failTime;
        private float completeTime;

        public Goal_GoTo(EnemyVehicle veh, Vector2 position) : base(veh)
        {
            destination = position;
        }

        public void FailAfter(float duration)
        {
            failAfter = true;
            failTime = duration;
        }
        public void CompleteAfter(float duration)
        {
            completeAfter = true;
            completeTime = duration;
        }

        public override void Activate()
        {
            base.Activate();

            veh.GotoPosition(destination, () => reached = true);
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (failAfter)
            {
                failTime -= veh.DeltaTime();
                if (failTime < 0)
                    status = Status.failed;
            }

            if (completeAfter)
            {
                completeTime -= veh.DeltaTime();
                if (completeTime < 0)
                    status = Status.completed;
            }

            if (reached)
            {
                status = Status.completed;
            }

            return status;
        }
    }
}
