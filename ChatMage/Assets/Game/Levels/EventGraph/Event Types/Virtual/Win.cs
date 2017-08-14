using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class Win : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Win The Game";

        public void Trigger()
        {
            Game.instance.levelScript.Win();
        }

        public override Color DefaultColor()
        {
            return new Color(0.95f, 0.65f, 0, 1);
        }

        public override string DefaultLabel()
        {
            return "Win The Game";
        }
    }
}
