using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class OnGameStart : PhysicalEvent
    {
        public Moment onGameStart = new Moment();
        private bool triggered = false;

        void Start()
        {
            if (Application.isPlaying)
            {
                triggered = false;
                if(Game.Instance != null)
                    HookOntoGame(Game.Instance);
            }
        }

        void Update()
        {
            if (Application.isPlaying)
            {
                if (Game.Instance != null)
                    HookOntoGame(Game.Instance);
            }
        }

        void HookOntoGame(Game game)
        {
            if (game.gameStarted)
                Trigger();
            else
                game.onGameStarted += Trigger;
        }

        public void Trigger()
        {
            if (triggered)
                return;
            onGameStart.Launch();
            enabled = false;
            triggered = true;
        }

        public override string NodeLabel()
        {
            return "Game Start";
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }
    }
}
