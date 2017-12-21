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
    public class WO_ExitScreen : StdWinOutro
    {
        public enum Direction { Up, Down, Right, Left }
        public Direction exitDirection = Direction.Up;
        
        private bool playerHasExited = false;
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

            LoadWinScene();

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

        protected override bool CanEnd()
        {
            return playerHasExited;
        }
    }
}
