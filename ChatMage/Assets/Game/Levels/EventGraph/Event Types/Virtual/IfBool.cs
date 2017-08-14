using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/if")]
    public class IfBool : VirtualEvent, IEvent
    {
        public Bool variable;
        public Moment _true = new Moment();
        public Moment _false = new Moment();

        public void Trigger()
        {
            if(variable != null)
            {
                if (variable.value)
                    _true.Launch();
                else
                    _false.Launch();
            }
        }

        public override string NodeLabel()
        {
            return "If " + (variable != null ? variable.name : "_____");
        }

        public override Color GUIColor()
        {
            GUI.contentColor = Color.black;
            return new Color(1, 1, 1, 1);
        }
    }
}
