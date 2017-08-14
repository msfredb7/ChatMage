using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Map/Reset Road"), DefaultNodeName("Road Reset")]
    public class ResetRoad : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            //...
        }

        public override Color GUIColor()
        {
            return new Color(0.65f, 0.65f, 1, 1);
        }

        public override string NodeLabel()
        {
            return "Reset Road";
        }
    }
}