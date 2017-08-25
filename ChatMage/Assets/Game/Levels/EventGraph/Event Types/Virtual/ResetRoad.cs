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
            Game.instance.map.mapping.GetTaggedObject("cadre").GetComponent<AjusteCadre>().Disappear();
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