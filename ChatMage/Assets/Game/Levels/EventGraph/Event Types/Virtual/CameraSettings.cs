using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace GameEvents
{
    [MenuItem("Camera/Settings"), DefaultNodeName("Cam Settings")]
    public class CameraSettings : FIVirtualEvent, IEvent
    {
        public bool modifyFollowPlayer;
        [InspectorShowIf("modifyFollowPlayer")]
        public bool followPlayerEffect = false;

        [InspectorMargin(5)]
        public bool modifyCanScrollUp = false;
        [InspectorShowIf("modifyCanScrollUp")]
        public bool canScrollUpEffect = false;

        [InspectorMargin(5)]
        public bool modifyCanScrollDown = false;
        [InspectorShowIf("modifyCanScrollDown")]
        public bool canScrollDownEffect = false;

        [InspectorMargin(5)]
        public bool resetCameraMax = false;
        public bool resetCameraMin = false;

        public void Trigger()
        {
            GameCamera cam = Game.instance.gameCamera;

            if (modifyFollowPlayer)
            {
                cam.followPlayer = followPlayerEffect;
            }

            if (modifyCanScrollUp)
            {
                cam.canScrollUp = canScrollUpEffect;
            }

            if (modifyCanScrollDown)
            {
                cam.canScrollDown = canScrollDownEffect;
            }

            if (resetCameraMax)
            {
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMaxToCamera();
            }

            if (resetCameraMin)
            {
                Game.instance.map.roadPlayer.CurrentRoad.ApplyMinToCamera();
            }
        }

        public override string NodeLabel()
        {
            return "Camera Settings";
        }

        public override Color GUIColor()
        {
            return new Color(1f, 0.75f, 0.9f, 1);
        }
    }
}
