using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AI
{
    public abstract class BaseGoal_Tween<T> : Goal<T> where T : EnemyVehicle
    {
        public bool rewindTweenOnCancel = true;
        public bool killTweenOnTerminate = true;
        public Func<bool> cancelCondition;

        protected Tween tween;
        protected Func<Tween> tweenGetter;

        public BaseGoal_Tween(T veh) : base(veh) { }

        public override void Activate()
        {
            base.Activate();

            if (tween == null && tweenGetter != null)
            {
                tween = tweenGetter();
            }

            if (tween == null)
            {
                status = Status.completed;
                return;
            }

            tween.Play();

            //Timescale listener
            veh.onTimeScaleChange += UpdateTimescale;
            UpdateTimescale(veh);
        }

        private void UpdateTimescale(Unit unit)
        {
            if (tween != null)
                tween.timeScale = unit.TimeScale;
        }

        public override Status Process()
        {
            ActivateIfInactive();

            if (IsActive())
            {
                if (tween == null || !tween.IsActive() || (cancelCondition != null && cancelCondition()))
                {
                    status = Status.completed;
                }
            }

            return status;
        }

        public override void ForceFailure()
        {
            if (rewindTweenOnCancel && tween != null && tween.IsActive())
                tween.Rewind(false);
            base.ForceFailure();
        }

        public override void Removed()
        {
            base.Removed();

            if (killTweenOnTerminate && tween != null && tween.IsActive())
                tween.Kill();

            //Remove timescale listener
            veh.onTimeScaleChange -= UpdateTimescale;
        }
    }
}
