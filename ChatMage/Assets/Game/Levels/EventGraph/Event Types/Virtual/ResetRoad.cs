using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class ResetRoad : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Road Reset";

        public void Trigger()
        {
            //...
        }

        public override Color DefaultColor()
        {
            return new Color(0.65f, 0.65f, 1, 1);
        }

        public override string DefaultLabel()
        {
            return "Reset Road";
        }
    }
}