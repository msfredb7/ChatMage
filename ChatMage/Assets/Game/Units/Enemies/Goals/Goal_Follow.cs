using UnityEngine;

namespace AI
{
    public class Goal_Follow : Goal<EnemyVehicle>
    {
        public bool stopOnLoseSight = true;

        private Unit target;
        private float reachDistanceSQR;

        private bool movementPrediction;
        private float thinkAheadLength;

        /// <summary>
        /// Va vers la target. Se complete lorsqu'on atteint un range necessaire. SANS prediction de movement de la target
        /// </summary>
        public Goal_Follow(EnemyVehicle veh, Unit target, float reachDistance) : base(veh)
        {
            movementPrediction = false;
            this.target = target;
            reachDistanceSQR = reachDistance * reachDistance;
        }

        /// <summary>
        /// Va vers la target. Se complete lorsqu'on atteint un range necessaire. AVEC prediction de movement de la target
        /// </summary>
        public Goal_Follow(EnemyVehicle veh, Unit target, float reachDistance, float thinkAheadLength) : base(veh)
        {
            movementPrediction = true;
            this.thinkAheadLength = thinkAheadLength;
            this.target = target;
            reachDistanceSQR = reachDistance * reachDistance;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (Unit.HasPresence(target))
            {
                bool isInRange = false;

                Vector2 meToTarget = target.Position - veh.Position;
                isInRange = meToTarget.sqrMagnitude <= reachDistanceSQR;

                if (movementPrediction)
                {
                    Vector2 meToPPredic = meToTarget;
                    if (target is MovingUnit)
                        meToPPredic += (target as MovingUnit).Speed * thinkAheadLength;

                    if (meToPPredic.sqrMagnitude <= reachDistanceSQR)
                        isInRange = true;
                }

                if (isInRange)
                {
                    status = Status.completed;
                }
                else
                {
                    veh.GotoPosition(target.Position);
                }
            }
            else
            {
                if (stopOnLoseSight)
                    veh.Stop();
                status = Status.failed;
            }

            return status;
        }

        public override void ForceFailure()
        {
            base.ForceFailure();
            if (stopOnLoseSight)
                veh.Stop();
        }
    }
}
