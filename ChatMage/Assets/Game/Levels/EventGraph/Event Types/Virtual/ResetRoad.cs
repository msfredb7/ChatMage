using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Map/Reset Road"), DefaultNodeName("Road Reset")]
    public class ResetRoad : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.gameCamera.followPlayer = true;
            Game.instance.gameCamera.canScrollUp = true;
            Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
        }

        public override Color GUIColor()
        {
            return new Color(0.65f, 0.65f, 1, 1);
        }

        public override string NodeLabel()
        {
            return "Reset Road";
        }
    }
}