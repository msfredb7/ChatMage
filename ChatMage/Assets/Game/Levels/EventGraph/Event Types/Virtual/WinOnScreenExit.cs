using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Win on screen exit"), DefaultNodeName("WinOnScreenExit")]
    public class WinOnScreenExit : VirtualEvent, IEvent
    {
        public enum Direction { Up, Down, Right, Left }
        public Direction exitDirection = Direction.Up;
        public bool setCameraMax = true;
        public bool setCameraMin = true;

        [NonSerialized]
        private bool playerHasExited = false;
        [NonSerialized]
        private Func<Vector2, bool> testPlayerExit;
        [NonSerialized]
        private Unit player;
        [NonSerialized]
        private RemoteUpdate updater;

        private const float CAR_HALF_LENGTH = 0.5f;

        public void Trigger()
        {
            //Setup updater
            if (updater == null)
            {
                updater = new GameObject("WinOnScreenExit Remote Updater").AddComponent<RemoteUpdate>();
                updater.updateAction = Update;
            }

            //Stop camera
            GameCamera cam = Game.instance.gameCamera;
            if (setCameraMax)
                cam.maxHeight = cam.Height;
            if (setCameraMin)
                cam.minHeight = cam.Height;

            //Get player
            player = Game.instance.Player != null ? Game.instance.Player.vehicle : null;

            //Set action
            switch (exitDirection)
            {
                default:
                case Direction.Up:
                    {
                        Game.instance.playerBounds.top.enabled = false;
                        float up = cam.Top + CAR_HALF_LENGTH;
                        testPlayerExit = (Vector2 playerPos) => playerPos.y >= up;
                    }
                    break;
                case Direction.Down:
                    {
                        Game.instance.playerBounds.bottom.enabled = false;
                        float down = cam.Bottom - CAR_HALF_LENGTH;
                        testPlayerExit = (Vector2 playerPos) => playerPos.y <= down;
                    }
                    break;
                case Direction.Right:
                    {
                        Game.instance.playerBounds.right.enabled = false;
                        float right = cam.Right + CAR_HALF_LENGTH;
                        testPlayerExit = (Vector2 playerPos) => playerPos.x >= right;
                    }
                    break;
                case Direction.Left:
                    {
                        Game.instance.playerBounds.left.enabled = false;
                        float left = cam.Left - CAR_HALF_LENGTH;
                        testPlayerExit = (Vector2 playerPos) => playerPos.x <= left;
                    }
                    break;
            }
        }

        private void Update()
        {
            //Check if player has exited the screen
            if (!playerHasExited && player != null && !player.IsDead && testPlayerExit != null)
            {
                if (testPlayerExit(player.Position))
                {
                    playerHasExited = true;
                    Game.instance.levelScript.Win();
                }
            }
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }

        public override string NodeLabel()
        {
            return "Win when exiting";
        }
    }

}