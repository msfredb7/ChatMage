using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace GameEvents
{
    public class CameraSettings : FIVirtualEvent
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

        public override void Trigger()
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
        }

        public override string DefaultLabel()
        {
            return "Camera Settings";
        }

        public override Color DefaultColor()
        {
            return new Color(1f, 0.75f, 0.9f, 1);
        }
    }
}
