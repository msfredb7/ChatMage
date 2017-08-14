using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class StopRoad : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Stop Road";

        public void Trigger()
        {
            Game.instance.gameCamera.followPlayer = false;
            Game.instance.gameCamera.canScrollUp = false;
        }

        public override Color DefaultColor()
        {
            return new Color(0.65f, 0.65f, 1, 1);
        }

        public override string DefaultLabel()
        {
            return "Stop Road";
        }
    }
}
