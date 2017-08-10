using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace GameEvents
{
    public class Delay : BaseVirtualEvent
    {
        public const string NODE_NAME = "Delay";

        public float delay = 0;
        public Moment moment = new Moment();

        public override void Trigger()
        {
            if (delay > 0)
            {
                Game.instance.events.AddDelayedAction(TriggerInstant, delay);
            }
            else
            {
                TriggerInstant();
            }
        }

        private void TriggerInstant()
        {
            moment.Launch();
        }



        //------------------Display------------------//

        public override Color DefaultColor()
        {
            return new Color(1, 0.8f, 1, 1);
        }

        public override string DefaultLabel()
        {
            return "+ " + ((float)((int)(delay * 100))) / 100 + "s";
        }
    }
}