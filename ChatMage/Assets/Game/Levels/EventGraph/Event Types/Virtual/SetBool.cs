using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/set"), DefaultNodeName("Set Bool")]
    public class SetBool : VirtualEvent, IEvent
    {
        public bool valueToSet;
        public Bool variable;
        public MomentBool onSet = new MomentBool();

        public void Trigger()
        {
            if (variable != null)
                variable.value = valueToSet;
        }

        public override string NodeLabel()
        {
            return "Set " + (variable != null ? variable.name : "_____");
        }

        public override Color GUIColor()
        {
            return new Color(1, 0.7f, 0.5f, 1);
        }
    }
}
