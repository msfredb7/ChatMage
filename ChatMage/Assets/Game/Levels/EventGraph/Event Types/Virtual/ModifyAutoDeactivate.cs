using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Units/Modify AutoDeactivation"), DefaultNodeName("Mod AutoDeactivation")]
    public class ModifyAutoDeactivate : VirtualEvent, IEvent<Unit>
    {
        public bool enabled = false;
        public bool overrideDefaultDeactivationRange = false;
        public float newDeactivationRange = 10;

        public void Trigger(Unit unit)
        {
            AutoDeactivation autoDeactivation = unit.GetComponent<AutoDeactivation>();
            if(autoDeactivation != null)
            {
                autoDeactivation.enabled = enabled;
                autoDeactivation.overrideDefaultDeactivationRange = overrideDefaultDeactivationRange;
                autoDeactivation.newDeactivationRange = newDeactivationRange;
            }
        }
        public override Color GUIColor()
        {
            return Colors.AI;
        }
    }
}
