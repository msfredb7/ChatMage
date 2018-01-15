using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class ArcherGoal_Shoot : Goal_LookAt
    {
        public ArcherGoal_Shoot(EnemyVehicle myVehicle, Unit target) : base(myVehicle, target ,-1)
        {
        }

        public override void Activate()
        {
            base.Activate();

            ArcherVehicle aVeh = veh as ArcherVehicle;

            aVeh.animator.ShootAnimation(ShootArrow, ForceCompletion);
        }


        void ShootArrow()
        {
            ArcherVehicle aVeh = veh as ArcherVehicle;

            ArcherArrow proj = Game.Instance.SpawnUnit(aVeh.arrowPrefab, aVeh.arrowLaunchLocation.position);

            proj.Init(veh, veh.WorldDirection2D(), veh.targets);

            aVeh.OnShoot();
        }
    }
}
