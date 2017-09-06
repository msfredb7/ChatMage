using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Map/Hide Cadre"), DefaultNodeName("HideCadre")]
    public class HideCadre : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.cadre.Disappear();
        }

        public override string NodeLabel()
        {
            return "Hide Cadre";
        }

        public override Color GUIColor()
        {
            return Colors.MAP;
        }
    }

}