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
    public class FromBottom : BaseIntro
    {
        private const string LOCK_KEY = "introlock";
        public float playerEnterDelay = 1.5f;

        public override void Play(Action onComplete)
        {
            Sequence sq = DOTween.Sequence();

            PlayerController player = Game.instance.Player;
            player.playerDriver.enableInput = false;

            Vehicle playerVehicle = player.vehicle;

            playerVehicle.TeleportPosition(new Vector2(0, Game.instance.gameCamera.Bottom - 2));
            playerVehicle.TeleportDirection(90);

            playerVehicle.wheelsOnTheGround.Lock(LOCK_KEY);
            playerVehicle.canTurn.Lock(LOCK_KEY);

            sq.Insert(playerEnterDelay, playerVehicle.transform.DOMoveY(Game.instance.gameCamera.Height, 3 - playerEnterDelay)
                .SetEase(Ease.OutSine)
                .OnComplete(delegate ()
                {
                    //Re-enable player things
                    playerVehicle.wheelsOnTheGround.Unlock(LOCK_KEY);
                    playerVehicle.canTurn.Unlock(LOCK_KEY);
                    player.playerDriver.enableInput = true;
                }));

            sq.InsertCallback(3, delegate ()
                {
                    if (onComplete != null)
                        onComplete();
                })
                .SetUpdate(false);
        }
    }
}
