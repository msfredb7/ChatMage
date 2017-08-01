using UnityEngine;

namespace AI
{
    public class Goal_Wander : Goal<EnemyVehicle>
    {
        float chooseInterval;
        float distanceMin;
        float distanceMax;

        float chooseTimer = 0;

        public Goal_Wander(EnemyVehicle veh, float chooseInterval =5f, float distanceMin = 0.75f, float distanceMax = 3.5f) : base(veh)
        {
            this.chooseInterval = chooseInterval;
            this.distanceMax = distanceMax;
            this.distanceMin = distanceMin;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            chooseTimer -= veh.DeltaTime();

            if(chooseTimer < 0)
            {
                Vector2 randomVector = Vehicle.AngleToVector(Random.Range(0, 360));

                veh.GotoPosition(veh.Position + randomVector * Random.Range(distanceMin, distanceMax));

                chooseTimer = Random.Range(chooseInterval * 0.75f, chooseInterval * 1.25f);
            }

            status = Status.active;

            return status;
        }
    }
}
