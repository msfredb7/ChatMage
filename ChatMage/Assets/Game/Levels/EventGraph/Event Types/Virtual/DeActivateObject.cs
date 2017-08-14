using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    /// <summary>
    /// A ENLEVER
    /// </summary>
    public class DeActivateObject : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Deactivate";

        public GameObject[] obj;

        public void Trigger()
        {
            foreach (var item in obj)
            {
                item.SetActive(false);
            }
        }

        public override Color GUIColor()
        {
            return new Color(1, 0.85f, 0.65f, 1);
        }

        public override string NodeLabel()
        {
            return "Deactivate";
        }
    }
}
