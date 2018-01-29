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
            Game.Instance.gameCamera.followPlayer = true;
            Game.Instance.gameCamera.canScrollUp = true;
            Game.Instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
            Game.Instance.cadre.Disappear();
        }

        public override Color GUIColor()
        {
            return Colors.MAP;
        }

        public override string NodeLabel()
        {
            return "Reset Road";
        }
    }
}