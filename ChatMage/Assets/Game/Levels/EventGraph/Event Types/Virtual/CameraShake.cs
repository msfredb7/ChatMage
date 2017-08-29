using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace GameEvents
{
    [MenuItem("Camera/Shake"), DefaultNodeName("Camera Shake")]
    public class CameraShake : FIVirtualEvent, IEvent
    {
        public bool cameraHit = false;
        [InspectorShowIf("cameraHit")]
        public Vector2 hitVector = Vector2.up * 1.2f;

        public bool cameraShake = false;
        [InspectorShowIf("cameraShake")]
        public float strength = 1.2f;
        [InspectorShowIf("cameraShake")]
        public float duration = 0;

        public void Trigger()
        {
            if (cameraShake)
            {
                if (duration > 0)
                    Game.instance.gameCamera.vectorShaker.Shake(strength, duration);
                else
                    Game.instance.gameCamera.vectorShaker.Shake(strength);
            }

            if (cameraHit)
            {
                Game.instance.gameCamera.vectorShaker.Hit(hitVector);
            }
        }

        public override Color GUIColor()
        {
            return Colors.CAMERA;
        }

        public override string NodeLabel()
        {
            return "Cam Shake";
        }
    }

}