using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
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

        public override Color DefaultColor()
        {
            return new Color(1, 0.85f, 0.65f, 1);
        }

        public override string DefaultLabel()
        {
            return "Deactivate";
        }
    }
}
