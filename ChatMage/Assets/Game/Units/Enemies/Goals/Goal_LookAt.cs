using UnityEngine;

namespace AI
{
    public class Goal_LookAt : Goal<EnemyVehicle>
    {
        private Unit target;
        private float minAngle;

        /// <summary>
        /// Va vers la target. Se complete lorsqu'on atteint un range necessaire. SANS prediction de movement de la target
        /// </summary>
        public Goal_LookAt(EnemyVehicle veh, Unit target, float minAngle = 4, bool stopVehicle = true) : base(veh)
        {
            this.target = target;
            this.minAngle = minAngle;
            if (stopVehicle)
                veh.Stop();
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (Unit.HasPresence(target))
            {
                Vector2 meToTarget = target.Position - veh.Position;
                float meToTargetAngle = meToTarget.ToAngle();

                if ((meToTargetAngle - veh.Rotation).Abs() < minAngle)
                {
                    status = Status.completed;
                }
                else
                {
                    veh.TurnToDirection(meToTargetAngle, veh.DeltaTime());
                }
            }
            else
            {
                status = Status.failed;
            }

            return status;
        }
    }
}
