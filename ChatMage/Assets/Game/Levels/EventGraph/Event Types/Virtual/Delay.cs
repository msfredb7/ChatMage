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
                Game.instance.events.AddDelayedAction(moment.Launch, delay);
            }
            else
            {
                moment.Launch();
            }
        }

        //------------------Display------------------//

        public override Color GUIColor()
        {
            return new Color(1, 0.8f, 1, 1);
        }

        public override string NodeLabel()
        {
            return "+ " + ((float)((int)(delay * 100))) / 100 + "s";
        }
    }
}