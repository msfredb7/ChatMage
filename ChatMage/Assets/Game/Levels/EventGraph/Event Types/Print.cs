using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class Print : VirtualEvent
    {
        public const string NODE_NAME = "Print";
        public string message;

        public override void Trigger()
        {
            Debug.Log(message);
        }

        public override Color DefaultColor()
        {
            return new Color(0.9f, 1, 1, 1);
        }
        public override string DefaultLabel()
        {
            string label = "Log: " + message;
            if(label.Length > 20)
            {
                label = label.Remove(18);
                label += "...";
            }
            return label;
        }
    }
}
