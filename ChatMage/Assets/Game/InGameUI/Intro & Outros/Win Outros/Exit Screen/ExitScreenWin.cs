using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EndGameReward;

namespace GameIntroOutro
{
    public class ExitScreenWin : BaseWinOutro
    {
        public enum Direction { Up, Down, Right, Left }
        public Direction exitDirection = Direction.Up;

        private EndGameRewardUI rewardUI;
        private bool playerHasExited = false;
        private bool hasEnded = false;
        private Func<Vector2, bool> testPlayerExit;
        private Unit player;

        private const float CarHalfLength = 0.5f;

        void Update()
        {
            if (!playerHasExited && player != null && !player.IsDead)
            {
                if (testPlayerExit(player.Position))
                {
                    playerHasExited = true;
                    CheckEnd();
                }
            }
        }

        public override void Play()
        {
            GameCamera cam = Game.instance.gameCamera;
            cam.followPlayer = false;
            player = Game.instance.Player != null ? Game.instance.Player.vehicle : null;

            Scenes.LoadAsync(EndGameRewardUI.SCENENAME, LoadSceneMode.Additive, OnWinScreenLoaded);

            switch (exitDirection)
            {
                default:
                case Direction.Up:
                    {
                        Game.instance.playerBounds.top.enabled = false;
                        float up = cam.Top + CarHalfLength;
                        testPlayerExit = (Vector2 playerPos) => playerPos.y >= up;
                    }
                    break;
                case Direction.Down:
                    {
                        Game.instance.playerBounds.bottom.enabled = false;
                        float down = cam.Bottom - CarHalfLength;
                        testPlayerExit = (Vector2 playerPos) => playerPos.y <= down;
                    }
                    break;
                case Direction.Right:
                    {
                        Game.instance.playerBounds.right.enabled = false;
                        float right = cam.Right + CarHalfLength;
                        testPlayerExit = (Vector2 playerPos) => playerPos.x >= right;
                    }
                    break;
                case Direction.Left:
                    {
                        Game.instance.playerBounds.left.enabled = false;
                        float left = cam.Left - CarHalfLength;
                        testPlayerExit = (Vector2 playerPos) => playerPos.x <= left;
                    }
                    break;
            }
        }

        void OnWinScreenLoaded(Scene scene)
        {
            rewardUI = Scenes.FindRootObject<EndGameRewardUI>(scene);

            rewardUI.Init(Game.instance.levelScript.rewards, Game.instance.levelScript.name);

            CheckEnd();
        }

        private void CheckEnd()
        {
            if (playerHasExited && rewardUI != null)
                End();
        }

        private void End()
        {
            if (hasEnded)
                return;
            hasEnded = true;

            Camera cam = Game.instance.gameCamera.cam;
            rewardUI.PinataHasBeenDestroyed(
                Game.instance.gameCamera.Center,
                cam,
                UnloadGameScenes);
        }

        private void UnloadGameScenes()
        {
            //On unload TOUS les scene sauf celle du endGameResult
            //   Peut etre qu'on devrais seulement unload les scene 'Framework' 'Map' et 'UI'
            //   Si on decide de faire ca, LE FAIRE DANS FRAMEWORK

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != EndGameRewardUI.SCENENAME)
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                    //i--;
                }
            }
        }
    }
}
