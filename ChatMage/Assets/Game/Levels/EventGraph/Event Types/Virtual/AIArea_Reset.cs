using UnityEngine;

namespace GameEvents
{
    public class AIArea_Reset : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "AI Area Reset";
        public void Trigger()
        {
            Game.instance.map.ResetAIArea();
        }

        public override Color DefaultColor()
        {
            return new Color(0.75f, 0.8f, 1f, 1);
        }

        public override string DefaultLabel()
        {
            return "Reset AI Area";
        }
    }
}
