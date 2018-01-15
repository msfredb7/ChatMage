using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class TonyGoal_FindSpot : Goal_Wander
    {
        private float roamTime;
        private Action<Vector2> zoneSetter;

        public TonyGoal_FindSpot(TonyVehicle veh, Action<Vector2> zoneSetter, float roamTime) : base(veh, 1.25f, 1.5f, 4.5f)
        {
            this.zoneSetter = zoneSetter;
            this.roamTime = roamTime;
        }

        public override Status Process()
        {
            if (status == Status.active && roamTime >= 0)
            {
                roamTime -= veh.DeltaTime();
            }
            return base.Process();
        }

        protected override void SetNewDestination()
        {
            if (roamTime < 0)
            {
                chooseTimer = float.PositiveInfinity;

                if (status != Status.completed)
                {
                    Vector2 dest = GetNewDestination();
                    dest = Game.Instance.aiArea.ClampToArea(dest, (veh as TonyVehicle).animator.UnitWidthWithZone);
                    Vector2 clampedDest = Game.Instance.map.VerifyPosition(dest, (veh as TonyVehicle).animator.UnitWidthWithZone);

                    zoneSetter(clampedDest);

                    status = Status.completed;
                }
            }
            else
            {
                base.SetNewDestination();
            }
        }
    }
}
