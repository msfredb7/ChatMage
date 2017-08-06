using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ArcherGoal_Shoot : Goal_LookAtTween
    {
        public ArcherGoal_Shoot(EnemyVehicle myVehicle, Unit target, Tween animation) : base(myVehicle, target ,animation)
        {
        }
        public ArcherGoal_Shoot(EnemyVehicle myVehicle, Unit target, Func<Tween> animationGetter) : base(myVehicle, target, animationGetter)
        {
        }

        public override void Activate()
        {
            base.Activate();

            if (!Unit.HasPresence(target))
                status = Status.failed;
        }
    }
}
