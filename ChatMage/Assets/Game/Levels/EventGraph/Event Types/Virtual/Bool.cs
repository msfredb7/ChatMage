using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class Bool : VirtualEvent
    {
        public bool value;

        public override Color DefaultColor()
        {
            return new Color(1, 0.85f, 0.5f, 1);
        }

        public override string DefaultLabel()
        {
            return "bool: " + name;
        }
    }
}