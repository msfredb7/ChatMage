using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameIntroOutro
{
    public class IntroStayInCenter : BaseIntro
    {
        [Header("General")]
        public float delay = 1;

        [Header("Camera")]
        public float cameraOrthographicSize = 2;
        public float unzoomDuration = 1.5f;


        public override void Play(Action onComplete)
        {
            //Camera

            GameCamera gameCamera = Game.instance.gameCamera;
            gameCamera.OrthoSize = cameraOrthographicSize;

            PlayerVehicle veh = Game.instance.Player.vehicle;

            veh.TeleportPosition(Game.instance.gameCamera.Center);
            veh.TeleportDirection(90);

            veh.canMove.Lock("intro");
            veh.canTurn.Lock("intro");

            gameCamera.DOOrthoSize(GameCamera.DEFAULT_SCREEN_HEIGHT / 2, unzoomDuration)
                .SetDelay(delay)
                .SetEase(Ease.InOutSine)
                .OnComplete(delegate ()
            {
                veh.canMove.Unlock("intro");
                veh.canTurn.Unlock("intro");

                onComplete();
            });
        }
    }
}
