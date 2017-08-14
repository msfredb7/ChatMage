using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Message"), DefaultNodeName("MsgToLvl")]
    public class MessageToLevel : VirtualEvent, IEvent
    {
        public string message;

        public void Trigger()
        {
            Game.instance.levelScript.ReceiveEvent(message);
        }

        public override string NodeLabel()
        {
            return "To Level: " + message;
        }

        public override Color GUIColor()
        {
            return new Color(0.7f, 0.7f, 0.7f, 1);
        }
    }
}
