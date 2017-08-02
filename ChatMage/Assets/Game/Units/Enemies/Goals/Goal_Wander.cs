using UnityEngine;

namespace AI
{
    public class Goal_Wander : Goal<EnemyVehicle>
    {
        protected float chooseInterval;
        protected float distanceMin;
        protected float distanceMax;

        protected float chooseTimer = 0;

        public Goal_Wander(EnemyVehicle veh, float chooseInterval = 3.5f, float distanceMin = 0.75f, float distanceMax = 3.5f) : base(veh)
        {
            this.chooseInterval = chooseInterval;
            this.distanceMax = distanceMax;
            this.distanceMin = distanceMin;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if(status == Status.active)
            {
                chooseTimer -= veh.DeltaTime();

                if (chooseTimer < 0)
                {
                    SetNewDestination();
                }
            }

            return status;
        }

        protected virtual void SetNewDestination()
        {
            veh.GotoPosition(GetNewDestination());
            chooseTimer = Random.Range(chooseInterval * 0.75f, chooseInterval * 1.25f);
        }

        protected virtual Vector2 GetNewDestination()
        {
            Vector2 randomVector = Vehicle.AngleToVector(Random.Range(0, 360));

            return veh.Position + randomVector * Random.Range(distanceMin, distanceMax);
        }
    }
}
