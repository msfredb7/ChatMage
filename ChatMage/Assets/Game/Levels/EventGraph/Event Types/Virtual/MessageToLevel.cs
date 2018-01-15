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
            Game.Instance.levelScript.ReceiveEvent(message);
        }

        public override string NodeLabel()
        {
            return "To Level: " + message;
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }
    }
}
