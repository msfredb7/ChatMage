using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Map/Stop Road"), DefaultNodeName("Stop Road")]
    public class StopRoad : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.gameCamera.followPlayer = false;
            Game.instance.gameCamera.canScrollUp = false;
        }

        public override Color GUIColor()
        {
            return new Color(0.65f, 0.65f, 1, 1);
        }

        public override string NodeLabel()
        {
            return "Stop Road";
        }
    }
}
