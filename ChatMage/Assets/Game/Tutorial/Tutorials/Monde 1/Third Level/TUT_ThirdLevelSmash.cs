using System.Collections;
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
            Milestone secondIntersection = Game.Instance.map.roadPlayer.CurrentRoad.milestones.Find(
             (x) => x.GameObj.name == "Second Intersection") as Milestone;

            Game.Instance.ui.smashDisplay.UseNotificationText = false;

            if (secondIntersection == null)
            {
                Debug.LogError("On a pas pu trouver la milestone 'Second Intersection'");
                End(false);
            }
            else
            {
                secondIntersection.onExecute.AddListener(OnReach2ndIntersection);
            }

            Game.Instance.Player.playerSmash.onSmashStarted += OnPlayerSmash;
        }

        private void OnPlayerSmash()
        {
            canLaunchTut = false;
        }

        void OnReach2ndIntersection()
        {
            PlayerController pc = Game.Instance.Player;
            if (pc != null)
            {
                pc.playerStats.OnUnitKilled += CheckLaunchTut;
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
            PlayerController pc = Game.Instance.Player;
            if (pc != null)
            {
                pc.playerStats.OnUnitKilled -= CheckLaunchTut;
            }

            Game.Instance.ui.smashDisplay.UseNotificationText = true;

            canLaunchTut = false;
            modules.delayedAction.Do(1, P1_ShowSmashMeter);
        }


        void P1_ShowSmashMeter()
        {
            Game.Instance.ui.playerInputs.Enabled.Lock("tut");

            modules.shorcuts.TimeFreeze();
            modules.textDisplay.SetBottom();
            modules.textDisplay.DisplayText("You have earned enough time-energy by defeating enemies. You can now use your "
                + "special power to <color=#96EEFFFF>SLOW DOWN TIME!</color>", true);
            modules.spotlight.On(Game.Instance.ui.smashDisplay.GetAbsolutePosition());

            modules.delayedAction.Do(1, () => modules.shorcuts.WaitForTouchOrKeyDown(P2_YouHaveEnough));
        }
        void P2_YouHaveEnough()
        {
            string message = Application.isMobilePlatform ? "Activate your power by touching the center of your screen."
                : "Activate your power by pressing the space bar.";
            modules.textDisplay.DisplayText(message, true);

            CanvasGroupBehaviour controlsDisplay = null;
            if (!Application.isMobilePlatform)
            {
                controlsDisplay = Instantiate(useSmashControls.gameObject, modules.transform).GetComponent<CanvasGroupBehaviour>();
                controlsDisplay.transform.localScale = Vector3.one;

                controlsDisplay.HideInstant();
                controlsDisplay.Show();
            }

            modules.delayedAction.Do(1, () =>
                modules.shorcuts.WaitForTouchOrKeyDown(() =>
                {
                    Game.Instance.ui.playerInputs.Enabled.Unlock("tut");
                    modules.shorcuts.TimeUnFreeze();
                    if (controlsDisplay != null)
                        controlsDisplay.Hide();


                    InitQueue queue = new InitQueue(End);
                    modules.spotlight.Off(queue.RegisterTween());
                    modules.textDisplay.HideText(queue.RegisterTween());
                    queue.MarkEnd();
                })
            );
        }
    }
}
