using UnityEngine;

namespace GameEvents
{
    [MenuItem("Units/Reset AI Area"), DefaultNodeName("AI Area Reset")]
    public class AIArea_Reset : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.map.ResetAIArea();
        }

        public override Color GUIColor()
        {
            return Colors.AI;
        }

        public override string NodeLabel()
        {
            return "Reset AI Area";
        }
    }
}
