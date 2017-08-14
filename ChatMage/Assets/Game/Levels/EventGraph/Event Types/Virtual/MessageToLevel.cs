using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class MessageToLevel : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "MsgToLvl";

        public string message;

        public void Trigger()
        {
            Game.instance.levelScript.ReceiveEvent(message);
        }

        public override string DefaultLabel()
        {
            return "To Level: " + message;
        }

        public override Color DefaultColor()
        {
            return new Color(0.7f, 0.7f, 0.7f, 1);
        }
    }
}
