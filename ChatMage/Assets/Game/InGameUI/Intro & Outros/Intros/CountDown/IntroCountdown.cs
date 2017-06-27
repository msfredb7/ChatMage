using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GameIntroOutro
{
    public class IntroCountdown : BaseIntro
    {
        private const string LOCK_KEY = "introlock";
        public Text text;
        public float playerEnterDelay = 1.5f;

        public override void Play(Action onComplete)
        {
            Sequence sq = DOTween.Sequence();

            PlayerController player = Game.instance.Player;
            player.playerDriver.enableInput = false;

            Vehicle playerVehicle = player.vehicle;

            playerVehicle.TeleportPosition(new Vector2(0, Game.instance.gameCamera.Bottom - 2));
            playerVehicle.TeleportDirection(90);

            playerVehicle.canUseBorder = false;
            playerVehicle.wheelsOnTheGround.Lock(LOCK_KEY);
            playerVehicle.canTurn.Lock(LOCK_KEY);

            sq.Insert(playerEnterDelay, playerVehicle.transform.DOMoveY(Game.instance.gameCamera.Height, 3 - playerEnterDelay)
                .SetEase(Ease.OutSine)
                .OnComplete(delegate ()
                {
                    //Re-enable player things
                    playerVehicle.canUseBorder = true;
                    playerVehicle.wheelsOnTheGround.Unlock(LOCK_KEY);
                    playerVehicle.canTurn.Unlock(LOCK_KEY);
                    player.playerDriver.enableInput = true;
                }));


            sq.InsertCallback(0, delegate () { text.text = "3"; })
                .InsertCallback(1, delegate () { text.text = "2"; })
                .InsertCallback(2, delegate () { text.text = "1"; })
                .InsertCallback(3, delegate ()
                {
                    text.enabled = false;

                    if (onComplete != null)
                        onComplete();
                })
                .InsertCallback(4, delegate () { Destroy(gameObject); })
                .SetUpdate(false);
        }
    }
}
