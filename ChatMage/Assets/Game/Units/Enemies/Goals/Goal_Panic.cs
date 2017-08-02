using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Goal_Panic : Goal<EnemyVehicle>
    {
        const float MIN_DIST = 1.5f;
        const float MAX_DIST = 3;
        const float NEW_DEST_EACH = 2.75f;

        private float lastDestinationTime;
        private InGameEvents events;

        public Goal_Panic(EnemyVehicle veh) : base(veh) { }

        public override void Activate()
        {
            base.Activate();

            events = Game.instance.events;

            NewDestination();
        }

        private void NewDestination()
        {
            if (IsActive())
            {
                veh.GotoPosition(CCC.Math.Vectors.RandomVector2(0, 360, MIN_DIST, MAX_DIST) + veh.Position, NewDestination);
                lastDestinationTime = events.GameTime;
            }
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (IsActive())
            {
                float timeSinceLastDest = events.GameTime - lastDestinationTime;
                if (timeSinceLastDest > NEW_DEST_EACH)
                    NewDestination();
            }

            return status;
        }
    }
}