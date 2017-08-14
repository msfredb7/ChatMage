using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Variables/bool/if")]
    public class IfBool : VirtualEvent, IEvent, IEvent<bool>
    {
        public Bool defaultVariable;
        public Moment _true = new Moment();
        public Moment _false = new Moment();

        public void Trigger()
        {
            if(defaultVariable != null)
            {
                if (defaultVariable.value)
                    _true.Launch();
                else
                    _false.Launch();
            }
        }

        public override string NodeLabel()
        {
            return "If " + (defaultVariable != null ? defaultVariable.name : "_____");
        }

        public override Color GUIColor()
        {
            GUI.contentColor = Color.black;
            return new Color(1, 1, 1, 1);
        }

        public void Trigger(bool a)
        {
            if (a)
                _true.Launch();
            else
                _false.Launch();
        }
    }
}
