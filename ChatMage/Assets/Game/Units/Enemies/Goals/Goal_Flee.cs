using UnityEngine;

namespace AI
{
    public class Goal_Flee : Goal<EnemyVehicle>
    {
        private Unit target;
        private float fleeDistSQR;
        private float minDuration;

        private float durationSoFar;

        public Goal_Flee(EnemyVehicle myVehicle, Unit target, float fleeDistance, float minDuration = 0.5f) : base(myVehicle)
        {
            this.target = target;
            fleeDistSQR = fleeDistance * fleeDistance;
            this.minDuration = minDuration;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (status == Status.active)
            {

                if (Unit.HasPresence(target))
                {
                    Vector2 meToTarget = target.Position - veh.Position;
                    if (meToTarget.sqrMagnitude > fleeDistSQR)
                    {
                        status = Status.completed;
                    }
                    else
                    {
                        veh.GotoDirection(veh.Position - target.Position, veh.DeltaTime());
                    }
                }
                else if (durationSoFar >= minDuration)
                {
                    status = Status.completed;
                }

                if (durationSoFar < minDuration)
                    durationSoFar += veh.DeltaTime();
            }

            return status;
        }
    }
}