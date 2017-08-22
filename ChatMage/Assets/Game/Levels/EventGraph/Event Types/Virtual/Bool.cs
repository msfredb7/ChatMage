using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/bool")]
    public class Bool : VirtualEvent,IEvent<bool>
    {
        public bool value;

        public override Color GUIColor()
        {
            return Colors.VARIABLES;
        }

        public override string NodeLabel()
        {
            return "bool: " + name;
        }

        public void Trigger(bool a)
        {
            value = a;
        }
    }
}