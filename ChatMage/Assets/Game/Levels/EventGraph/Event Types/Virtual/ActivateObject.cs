using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class ActivateObject : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Activate";

        public GameObject[] obj;

        public void Trigger()
        {
            foreach (var item in obj)
            {
                item.SetActive(true);
            }
        }

        public override Color DefaultColor()
        {
            return new Color(0.85f, 1, 0.65f, 1);
        }

        public override string DefaultLabel()
        {
            return "Activate";
        }
    }
}
