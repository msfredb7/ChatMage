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

        [Header("Portal")]
        public float cameraOrthographicSize = 2;
        public float unzoomDuration = 1.5f;

        [Header("Portal")]
        public Transform portal;
        public Vector2 cameraSideOffset;
        public float portalOpenningDelay = 1;
        public float portalOpenningDuration = 0.75f;
        public float portalClosingDuration = 0.75f;

        [Header("Player Entrance")]
        public float playerEntranceDelay = 1.5f;
        public float playerEntranceDuration = 1f;
        public float playerFinalX = -3;

        public override void Play(Action onComplete)
        {

            //On commence avec la camera zoomed in
            GameCamera gCam = Game.instance.gameCamera;
            gCam.OrthoSize = cameraOrthographicSize;

            //Portail fermer
            portal.localScale = Vector3.zero;

            //Get player
            PlayerController player = Game.instance.Player;
            Vehicle veh = player.vehicle;

            //Position + Direction
            Vector2 camRightBorder = Game.instance.gameCamera.Center + Vector2.right * gCam.OrthoSize * gCam.cam.aspect;
            veh.TeleportPosition(camRightBorder + Vector2.right * 2);
            veh.TeleportDirection(180);

            //Lock player
            veh.wheelsOnTheGround.Lock(LOCK_KEY);
            veh.canTurn.Lock(LOCK_KEY);

            Sequence sq = DOTween.Sequence().OnComplete(
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

            //Position portal
            portal.position = cameraSideOffset + camRightBorder;
            //Open portal
            sq.Append(portal.DOScale(1, portalOpenningDuration).SetEase(Ease.OutBack));

            //Delay
            sq.AppendInterval(playerEntranceDelay);

            //Player arrival
            sq.Append(veh.transform.DOMoveX(playerFinalX, playerEntranceDuration)
                .SetEase(Ease.OutSine));

            //Portal closing
            sq.Append(portal.DOScale(0, portalClosingDuration).SetEase(Ease.InBack));

            //Unzoom
            sq.Join(gCam.DOOrthoSize(gCam.DefaultOrthoSize, unzoomDuration).SetEase(Ease.InOutSine));
        }
    }

}