using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Flow Control/Counter"), DefaultNodeName("Counter")]
    public class Counter : VirtualEvent, IEvent
    {
        public int i = 1;

        public void Trigger()
        {

        }

        public override string NodeLabel()
        {
            return "Counter";
        }
    }
}
