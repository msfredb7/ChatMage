using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace GameEvents
{
    [MenuItem("Other/Delay"), DefaultNodeName("Delay")]
    public class Delay : VirtualEvent, IEvent
    {
        public float delay = 0;
        public Moment moment = new Moment();

        public void Trigger()
        {
            if (delay > 0)
            {
                Game.Instance.events.AddDelayedAction(moment.Launch, delay);
            }
            else
            {
                moment.Launch();
            }
        }

        //------------------Display------------------//

        public override Color GUIColor()
        {
            return Colors.DELAY;
        }

        public override string NodeLabel()
        {
            return "+ " + ((float)((int)(delay * 100))) / 100 + "s";
        }
    }
}