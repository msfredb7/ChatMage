using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Debug/Print"), DefaultNodeName("Print")]
    public class Print : VirtualEvent, IEvent
    {
        public string message;

        public void Trigger()
        {
            Debug.Log(message);
        }

        public override Color GUIColor()
        {
            return new Color(0.9f, 1, 1, 1);
        }
        public override string NodeLabel()
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
