using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Flow Control/Counter"), DefaultNodeName("Counter")]
    public class Counter : VirtualEvent, IEvent
    {
        public int i = 0;
        public int every = 3;
        private Moment moment = new Moment();
        private Moment non_moment = new Moment();

        public void Trigger()
        {
            i++;
            if (i >= every)
            {
                moment.Launch();
                i = 0;
            }
            else
            {
                non_moment.Launch();
            }
        }

        public override string NodeLabel()
        {
            return "Counter";
        }

        public override void GetAdditionalMoments(out BaseMoment[] moments, out string[] names)
        {
            moments = new BaseMoment[2];
            names = new string[2];

            moments[0] = moment;
            moments[1] = non_moment;

            names[0] = "every " + every;
            names[1] = "other";
        }

        public override Color GUIColor()
        {
            return Colors.FLOW_CONTROL;
        }
    }
}
