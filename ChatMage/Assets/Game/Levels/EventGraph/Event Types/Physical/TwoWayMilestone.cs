using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class TwoWayMilestone : PhysicalEvent, IMilestone
    {
        public Moment onGoingUp = new Moment();
        public Moment onGoingDown = new Moment();


        public bool gizmosAlwaysVisible = true;
        [Header("Trigger")]
        public MSTriggerType triggerOn = MSTriggerType.TopOfScreen;
        public bool disapearAfterTrigger = false;

        public MSTriggerType TriggerOn { get { return triggerOn; } }

        public GameObject GameObj { get { return gameObject; } }

        public override Color GUIColor()
        {
            return Colors.MILESTONE;
        }

        public override string NodeLabel()
        {
            return "TWMS: " + name;
        }

        public bool Execute(bool isGoingUp)
        {
            if (!enabled)
                return false;

            if (isGoingUp)
                onGoingUp.Launch();
            else
                onGoingDown.Launch();

            return disapearAfterTrigger;
        }

        public void Disable()
        {
            enabled = false;
        }

        public float GetVirtualHeight()
        {
            return transform.position.y + (triggerOn == MSTriggerType.BottomOfScreen ? GameCamera.DEFAULT_SCREEN_HEIGHT : 0);
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
            Gizmos.color = new Color(triggerOn == MSTriggerType.BottomOfScreen ? 1 : 0, triggerOn == MSTriggerType.TopOfScreen ? 1 : 0, 0, 1);
            Gizmos.DrawCube(transform.position, new Vector3(GameCamera.DEFAULT_SCREEN_WIDTH, disapearAfterTrigger ? 0.25f : 0.5f, 1));
        }
    }
}
