using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameIntroOutro
{
    public class IntroFromPortal : BaseIntro
    {
        private const string LOCK_KEY = "introlock";

        [Header("Camera")]
        public float cameraOrthographicSize = 2;
        public float unzoomDuration = 1.5f;

        [Header("Portal Entrance")]
        public float portalOpenningDelay = 1;
        public PortalVFX portal;
        public Vector2 cameraSideOffset;

        [Header("Camera Move")]
        public float cameraMoveDelay = 1;
        public float cameraMoveDuration = 1;

        [Header("Player Entrance")]
        public float playerEntranceDelay = 1.5f;
        public float playerEntranceDuration = 1f;
        public float playerFinalX = -3;

        [Header("Portal Exit")]
        public float portalCloseDelay = 0.75f;

        private Sequence sq;

        public override void Play(Action onComplete)
        {

            //On commence avec la camera zoomed in
            GameCamera gCam = Game.instance.gameCamera;
            //Camera ortho
            gCam.OrthoSize = cameraOrthographicSize;
            //Camera wiggle
            float wiggleSize = gCam.wiggler.GetAnimationSize();
            gCam.wiggler.SetAnimationSize(0, 0);
            //Camera Position
            Vector3 camPos = gCam.transform.position;
            //Disable Camera
            gCam.enabled = false;

            //Get player
            PlayerController player = Game.instance.Player;
            Vehicle veh = player.vehicle;

            Vector2 camRightBorder = Game.instance.gameCamera.Center + Vector2.right * gCam.OrthoSize * gCam.cam.aspect;

            //Position portal
            Vector2 portalPos = cameraSideOffset + camRightBorder;
            portal.transform.position = portalPos;

            //Position Camera
            gCam.transform.position = new Vector3(portalPos.x, portalPos.y, camPos.z);

            //Position player + Direction
            veh.TeleportPosition(camRightBorder + Vector2.right * 5);
            //veh.gameObject.SetActive(false);
            veh.TeleportDirection(180);

            //Lock player
            veh.wheelsOnTheGround.Lock(LOCK_KEY);
            veh.canTurn.Lock(LOCK_KEY);

            sq = DOTween.Sequence().OnComplete(
                delegate ()
                {
                    //Unlock player
                    veh.wheelsOnTheGround.Unlock(LOCK_KEY);
                    veh.canTurn.Unlock(LOCK_KEY);

                    if (onComplete != null)
                        onComplete();
                });

            //Delay
            sq.AppendInterval(portalOpenningDelay);

            //Open portal
            sq.AppendCallback(portal.Open);

            //Delay
            sq.AppendInterval(cameraMoveDelay);

            //Move Camera
            sq.Append(gCam.transform.DOMove(camPos, cameraMoveDuration).SetEase(Ease.InOutSine));

            //Delay
            sq.AppendCallback(() => veh.TeleportPosition(camRightBorder + Vector2.right * 1));
            sq.AppendInterval(playerEntranceDelay);

            //Player arrival
            //sq.AppendCallback(() => veh.gameObject.SetActive(true));
            sq.Append(veh.transform.DOMoveX(playerFinalX, playerEntranceDuration)
                .SetEase(Ease.OutSine));

            //Delay
            sq.AppendInterval(portalCloseDelay);

            //Portal closing
            sq.AppendCallback(portal.Close);

            //Reset camera status
            sq.AppendCallback(() =>
            {
                gCam.enabled = true;
                gCam.wiggler.SetAnimationSize(wiggleSize, 1);
            });

            //Unzoom
            sq.Join(gCam.DOOrthoSize(gCam.DefaultOrthoSize, unzoomDuration).SetEase(Ease.InOutSine));
        }

        private void OnDestroy()
        {
            if (sq != null && sq.IsActive())
                sq.Kill();
        }
    }

}