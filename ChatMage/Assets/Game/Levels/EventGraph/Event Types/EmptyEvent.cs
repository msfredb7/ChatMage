using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class EmptyEvent : VirtualEvent
    {
        public Moment onTrigger = new Moment();

        public override void Trigger()
        {
            onTrigger.Launch();
        }

        public override string DefaultLabel()
        {
            return "Empty";
        }
    }
}
