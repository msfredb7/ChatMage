using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FullSerializer;

namespace Tutorial
{
    public class TUT_FirstLevel : BaseTutorial
    {
        public CanvasGroup handClickPrefab;


        [NonSerialized, fsIgnore]
        private bool hasStarted = false;

        protected override void Cleanup()
        {
            if (hasStarted)
            {
                Game.instance.currentLevel.onEventReceived -= CurrentLevel_onEventReceived;
                Game.instance.onUnitSpawned -= TutoEnemy;
            }
        }

        protected override void OnStart()
        {
            if (Game.instance == null)
            {
                Debug.LogError("Game is null ?");
                End(false);
                return;
            }

            hasStarted = true;
            Game.instance.currentLevel.onEventReceived += CurrentLevel_onEventReceived;
            Game.instance.onUnitSpawned += TutoEnemy;
        }

        private void CurrentLevel_onEventReceived(string text)
        {
            switch (text)
            {
                case "tuto move":
                    TutoMove1();
                    break;
            }
        }

        void TutoMove1()
        {
            modules.shorcuts.TimeFreeze();
            modules.textDisplay.SetBottom();
            modules.textDisplay.DisplayText("Turn your vehicle to the left by clicking on the left side of your screen", true);

            Image handClick = Instantiate(handClickPrefab.gameObject, modules.transform).GetComponent<Image>();

            Vector2 leftSide = new Vector2(0.166665f, 0.5f);
            RectTransform tr = handClick.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0, 0);
            tr.anchorMax = new Vector2(0.333f, 1);
            tr.anchoredPosition = Vector2.zero;
            tr.sizeDelta = Vector2.zero;

            //Fade in la main
            handClick.GetComponent<CanvasGroup>().DOFade(1, 1).SetUpdate(true);

            //Flash le background
            handClick.DOFade(0.15f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);


            modules.delayedAction.Do(0.75f,
                delegate ()
                {
                    modules.proxyButton.ProxyScreen(
                        delegate ()
                        {
                            TutoMove2(handClick);
                        });
                });
        }

        void TutoMove2(Image handClick)
        {
            modules.textDisplay.DisplayText("Turn your vehicle to the right by clicking on the right side of your screen", true);

            RectTransform tr = handClick.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0.6667f, 0);
            tr.anchorMax = new Vector2(1, 1);
            tr.anchoredPosition = Vector2.zero;
            tr.sizeDelta = Vector2.zero;

            modules.delayedAction.Do(0.75f,
                delegate ()
                {
                    modules.proxyButton.ProxyScreen(
                        delegate ()
                        {
                            Destroy(handClick.gameObject);
                            modules.shorcuts.TimeUnFreeze();
                            modules.textDisplay.HideText();
                        });
                });
        }

        void TutoEnemy(Unit unit)
        {
            modules.delayedAction.Do(1.5f, delegate ()
            {
                modules.shorcuts.TimeFreeze();
                if (unit != null)
                    modules.spotlight.OnWorld(unit.Position);
                else
                    modules.spotlight.OnWorld(Game.instance.Player.vehicle.Position);

                modules.textDisplay.DisplayText("Defeat your enemies hitting them with your car."
                    + " But be careful, they will try to attack you to!", true);

                modules.delayedAction.Do(0.75f,
                    delegate ()
                    {
                        modules.proxyButton.ProxyScreen(delegate ()
                        {
                            modules.shorcuts.TimeUnFreeze();
                            modules.spotlight.Off();
                            modules.textDisplay.HideText();
                            End(true);
                        });
                    });
            });

            Game.instance.onUnitSpawned -= TutoEnemy;
        }
    }

}