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
            Game.Instance.gameCamera.followPlayer = false;
            Game.Instance.gameCamera.canScrollUp = false;
        }

        public override Color GUIColor()
        {
            return Colors.MAP;
        }

        public override string NodeLabel()
        {
            return "Stop Road";
        }
    }
}
