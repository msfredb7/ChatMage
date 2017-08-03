using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AI
{
    public class Goal_Tween : BaseGoal_Tween<EnemyVehicle>
    {
        public bool failOnInterruption = true;
        public bool stopVehicle = false;
        public Goal_Tween(EnemyVehicle myVehicle, Tween animation) : base(myVehicle)
        {
            tween = animation;
            tween.Pause();
        }
        public Goal_Tween(EnemyVehicle myVehicle, Func<Tween> animationGetter) : base(myVehicle)
        {
            this.tweenGetter = animationGetter;
        }

        public override void Activate()
        {
            base.Activate();

            if (stopVehicle)
                veh.Stop();
        }

        public override void Interrupted()
        {
            base.Interrupted();

            if (failOnInterruption)
                ForceFailure();
        }
    }
}
