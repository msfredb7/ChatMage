using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class BardGoal_Sing : BaseGoal_Tween<BardVehicle>, IShaker
    {
        public BardGoal_Sing(BardVehicle veh) : base(veh)
        {
            CanBeInterrupted = false;
        }

        public override void Activate()
        {
            tween = veh.animator.SingAnimation(OnSingBegin, OnSingEnd);
            veh.Stop();
            base.Activate();
        }

        private void OnSingBegin()
        {
            Game.Instance.gameCamera.vectorShaker.AddShaker(this);

            Allegiance[] allowedAlligiances = veh.boostTargets.targetAllegiances.ToArray();

            List<Unit> hitUnits = UnitDetection.OverlapCircleUnits(veh.Position, veh.singRadius, veh, allowedAlligiances);
            
            for (int i = 0; i < hitUnits.Count; i++)
            {
                float cap = hitUnits[i] is BardVehicle ? veh.capOnOtherBards : veh.defaultCap;//3

                float multiplier = veh.timeScaleMultiplier;
                float timescale = hitUnits[i].TimeScale;

                multiplier = Mathf.Max(1, Mathf.Min(cap / timescale, multiplier));

                if (multiplier > 1.001f)
                    hitUnits[i].AddBuff(new TimeScaleBuff(multiplier, veh.duration, true));
            }
        }

        private void OnSingEnd()
        {
            Game.Instance.gameCamera.vectorShaker.RemoveShaker(this);
        }

        public float GetShakeStrength()
        {
            return 0.1f;
        }
    }
}
