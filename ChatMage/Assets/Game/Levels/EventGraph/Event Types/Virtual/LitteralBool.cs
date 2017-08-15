using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/Litteral"), DefaultNodeName("Litteral Bool")]
    public class LitteralBool : VirtualEvent, IEvent
    {
        public bool value;
        public MomentBool boolean = new MomentBool();

        public void Trigger()
        {
            boolean.Launch(value);
        }

        public override string NodeLabel()
        {
            return "Bool: " + value;
        }

        public override Color GUIColor()
        {
            return new Color(1, 0.7f, 0.5f, 1);
        }
    }
}
