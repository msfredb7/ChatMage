using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Goal_LookAtTween : Goal_Tween
    {
        protected Unit target;
        public Goal_LookAtTween(EnemyVehicle myVehicle, Unit target, Tween animation) : base(myVehicle, animation)
        {
            stopVehicle = true;
            this.target = target;
        }
        public Goal_LookAtTween(EnemyVehicle myVehicle, Unit target, Func<Tween> animationGetter) : base(myVehicle, animationGetter)
        {
            stopVehicle = true;
            this.target = target;
        }

        public override Status Process()
        {
            if (status == Status.active && Unit.HasPresence(target))
            {
                veh.TurnToDirection(target.Position - veh.Position, veh.DeltaTime());
            }

            return base.Process();
        }
    }
}
