using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class EmptyEvent : VirtualEvent, IEvent
    {
        public Moment onTrigger = new Moment();

        public void Trigger()
        {
            onTrigger.Launch();
        }

        public override string DefaultLabel()
        {
            return "Empty";
        }
    }
}
