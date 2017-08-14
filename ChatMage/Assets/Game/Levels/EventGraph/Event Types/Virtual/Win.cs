using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Win"), DefaultNodeName("Win")]
    public class Win : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.levelScript.Win();
        }

        public override Color GUIColor()
        {
            return new Color(0.95f, 0.65f, 0, 1);
        }

        public override string NodeLabel()
        {
            return "Win the Game";
        }
    }
}
