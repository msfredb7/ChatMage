using UnityEngine;

namespace GameEvents
{
    public class AIArea_Set : PhysicalEvent, IEvent
    {
        public Box2D area = new Box2D(
            new Vector2(-GameCamera.DEFAULT_SCREEN_WIDTH / 2, -GameCamera.DEFAULT_SCREEN_HEIGHT),
            new Vector2(GameCamera.DEFAULT_SCREEN_WIDTH / 2, 0f));


        public bool gizmosAlwaysVisible = true;

        public void Trigger()
        {
            Game.Instance.aiArea.SetArea(area);
        }

        public override Color GUIColor()
        {
            return Colors.AI;
        }

        public override string NodeLabel()
        {
            return "Set AI Area";
        }

        void OnDrawGizmosSelected()
        {
            if (!gizmosAlwaysVisible)
                DrawGizmos();
        }

        void OnDrawGizmos()
        {
            if (gizmosAlwaysVisible)
                DrawGizmos();
        }

        void DrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 1, 0.4f);
            Gizmos.DrawCube((Vector2)transform.position + area.Center, area.Size);
        }
    }
}
