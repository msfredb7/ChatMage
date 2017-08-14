using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class SetBool : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Set Bool";

        public bool valueToSet;
        public Bool variable;

        public void Trigger()
        {
            if (variable != null)
                variable.value = valueToSet;
        }

        public override string DefaultLabel()
        {
            return "Set " + (variable != null ? variable.name : "_____");
        }

        public override Color DefaultColor()
        {
            return new Color(1, 0.7f, 0.5f, 1);
        }
    }
}
