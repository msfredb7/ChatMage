using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Camera/Wiggle"), DefaultNodeName("Cam Wiggle")]
    public class CamWiggle : VirtualEvent, IEvent
    {
        [Range(0, 0.2f)]
        public float animationSize = 0;
        public float transitionDuration = 2;

        public void Trigger()
        {
            Game.Instance.gameCamera.wiggler.SetAnimationSize(animationSize, transitionDuration);
        }

        public override Color GUIColor()
        {
            return Colors.CAMERA;
        }

        public override string NodeLabel()
        {
            return "CamWiggle: " + animationSize.Rounded(2);
        }
    }

}