using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Other/Empty")]
    public class EmptyEvent : VirtualEvent, IEvent
    {
        public Moment onTrigger = new Moment();

        public void Trigger()
        {
            onTrigger.Launch();
        }
    }
}
