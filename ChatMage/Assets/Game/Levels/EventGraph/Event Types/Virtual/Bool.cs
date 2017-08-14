using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/bool")]
    public class Bool : VirtualEvent
    {
        public bool value;

        public override Color GUIColor()
        {
            return new Color(1, 0.85f, 0.5f, 1);
        }

        public override string NodeLabel()
        {
            return "bool: " + name;
        }
    }
}