using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameEvents
{
    [MenuItem("Camera/Zoom"), DefaultNodeName("Camera Zoom")]
    public class CameraZoom : VirtualEvent, IEvent
    {
        public bool independantTimescale = true;

        [Header("Grosseur cible de la camera, 1 = normal"), Range(0.1f, 5)]
        public float targetSize = 1;
        public float animationDuration = .8f;
        public Ease ease = Ease.InOutSine;

        public void Trigger()
        {
            GameCamera cam = Game.instance.gameCamera;
            float size = (GameCamera.DEFAULT_SCREEN_HEIGHT / 2) * targetSize;

            if(animationDuration > 0)
            {
                cam.DOOrthoSize(size, animationDuration).SetEase(ease).SetUpdate(independantTimescale);
            }
            else
            {
                cam.OrthoSize = size;
            }
        }

        public override Color GUIColor()
        {
            return Colors.CAMERA;
        }

        public override string NodeLabel()
        {
            return "Cam zoom: " + targetSize;
        }
    }
}