using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class MessageToLevel : VirtualEvent, IEvent
    {
        public string message;

        public void Trigger()
        {
            Game.instance.levelScript.ReceiveEvent(message);
        }

        public override string DefaultLabel()
        {
            return "Message to Level";
        }

        public override Color DefaultColor()
        {
            return new Color(0.7f, 0.7f, 0.7f, 1);
        }
    }
}
