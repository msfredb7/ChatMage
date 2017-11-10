﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
namespace Tutorial
{
    public class TUT_ThirdLevelSmash : BaseTutorial
    {
        public CanvasGroupBehaviour useSmashControls;

        [NotSerialized]
        private bool canLaunchTut;
        [NotSerialized]
        private int killCount = 0;

        protected override void OnStart()
        {
            canLaunchTut = true;
            killCount = 0;

            //On listen a quand le joueur arrive a la 2e intersection
            Milestone secondIntersection = Game.instance.map.roadPlayer.CurrentRoad.milestones.Find(
             (x) => x.GameObj.name == "Second Intersection") as Milestone;

            if (secondIntersection == null)
            {
                Debug.LogError("On a pas pu trouver la milestone 'Second Intersection'");
                End(false);
            }
            else
            {
                secondIntersection.onExecute.AddListener(OnReach2ndIntersection);
            }

            Game.instance.Player.playerSmash.onSmashStarted += OnPlayerSmash;
        }

        private void OnPlayerSmash()
        {
            canLaunchTut = false;
        }

        void OnReach2ndIntersection()
        {
            PlayerController pc = Game.instance.Player;
            if (pc != null)
            {
                pc.playerStats.onUnitKilled += CheckLaunchTut;
            }
        }

        void CheckLaunchTut(Unit unitKilled)
        {
            killCount++;
            if (canLaunchTut && killCount >= 2)
            {
                LaunchTut(unitKilled);
            }
        }

        void LaunchTut(Unit unitKilled)
        {
            //Remove listener
            PlayerController pc = Game.instance.Player;
            if (pc != null)
            {
                pc.playerStats.onUnitKilled -= CheckLaunchTut;
            }

            canLaunchTut = false;
            modules.delayedAction.Do(1, P1_ShowSmashMeter);
        }

        void P1_ShowSmashMeter()
        {
            modules.shorcuts.TimeFreeze();
            modules.textDisplay.SetBottom();
            modules.textDisplay.DisplayText("You have earned enough time-energy by defeating enemies. You can now use your "
                + "special power to <color=#96EEFFFF>SLOW DOWN TIME!</color>", true);
            modules.spotlight.On(Game.instance.ui.smashDisplay.GetAbsolutePosition());

            modules.shorcuts.WaitForTouchOrKeyDown(P2_YouHaveEnough);
        }
        void P2_YouHaveEnough()
        {
            string message = Application.isMobilePlatform ? "Activate your power by touching the center of your screen."
                : "Activate your power by pressing one of those keys.";
            modules.textDisplay.DisplayText(message, true);

            CanvasGroupBehaviour controlsDisplay = Instantiate(useSmashControls.gameObject, modules.transform).GetComponent<CanvasGroupBehaviour>();
            useSmashControls.transform.localScale = Vector3.one;

            controlsDisplay.HideInstant();
            controlsDisplay.Show();

            modules.shorcuts.WaitForTouchOrKeyDown(() =>
            {
                modules.shorcuts.TimeUnFreeze();
                controlsDisplay.Hide();


                InitQueue queue = new InitQueue(End);
                modules.spotlight.Off(queue.RegisterTween());
                modules.textDisplay.HideText(queue.RegisterTween());
                queue.MarkEnd();
            });
        }
    }
}
