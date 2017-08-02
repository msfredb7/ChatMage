using UnityEngine;

namespace AI
{
    public class Goal_Flee : Goal<EnemyVehicle>
    {
        private Unit target;
        private float fleeDistSQR;

        public Goal_Flee(EnemyVehicle myVehicle, Unit target, float fleeDistance) : base(myVehicle)
        {
            this.target = target;
            fleeDistSQR = fleeDistance * fleeDistance;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (Unit.HasPresence(target))
            {
                Vector2 meToTarget = target.Position - veh.Position;
                if(meToTarget.sqrMagnitude > fleeDistSQR)
                {
                    status = Status.completed;
                }
                else
                {
                    veh.GotoDirection(veh.Position - target.Position, veh.DeltaTime());
                }
            }
            else
            {
                status = Status.completed;
            }

            return status;
        }
    }
}