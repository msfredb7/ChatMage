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
        public CanvasGroupBehaviour turnRightOrLeftPrefab;
        public CanvasGroup handheldPrefab;
        public CanvasGroup keyboardControlsPrefab;

        [NonSerialized, fsIgnore]
        private bool hasStarted = false;

        private bool isMobile;

        protected override void Cleanup()
        {
            if (hasStarted)
            {
                currentLevel.onEventReceived -= CurrentLevel_onEventReceived;
                Game.Instance.onUnitSpawned -= TutoEnemy;
            }
        }

        protected override void OnStart()
        {
            if (Game.Instance == null)
            {
                Debug.LogError("Game is null ?");
                End(false);
                return;
            }

            isMobile = Application.isMobilePlatform;
            hasStarted = true;
            Game.Instance.levelScript.onEventReceived += CurrentLevel_onEventReceived;
            Game.Instance.onUnitSpawned += TutoEnemy;
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

            Image handClick = null;
            CanvasGroupBehaviour turnRightOrLeft = null;
            CanvasGroup handheld = null;
            CanvasGroup pcControls = null;

            Action onComplete = () =>
            {
                if (turnRightOrLeft != null)
                    turnRightOrLeft.Hide();
                if (handClick != null)
                {
                    CanvasGroup cg = handClick.GetComponent<CanvasGroup>();
                    cg.DOKill();
                    cg.DOFade(0, 0.5f).SetUpdate(true);
                }

                //modules.shorcuts.TimeUnFreeze();
                //modules.textDisplay.HideText();
                //modules.spotlight.Off();
                //modules.delayedAction.Do(2, () => TutoMove2(handClick, turnRightOrLeft));
                TutoMove2(handClick, turnRightOrLeft, handheld, pcControls);
            };

            if (isMobile)
            {
                modules.textDisplay.DisplayText("Turn left by clicking on the left side of your screen", true);

                //modules.spotlight.FillCenter(true, true);
                //modules.spotlight.On();

                handClick = Instantiate(handClickPrefab.gameObject, modules.transform).GetComponent<Image>();
                handheld = Instantiate(handheldPrefab.gameObject, modules.transform).GetComponent<CanvasGroup>();

                RectTransform tr = handClick.GetComponent<RectTransform>();
                tr.localScale = Vector3.one;
                tr.anchorMin = new Vector2(0, 0);
                tr.anchorMax = new Vector2(0.333f, 1);
                tr.anchoredPosition = Vector2.zero;
                tr.sizeDelta = Vector2.zero;

                // Fade in handheld, et on le flip du bon coté
                handheld.alpha = 0;
                handheld.DOFade(1, 1).SetUpdate(true);
                handheld.transform.localScale = new Vector3(-1, 1, 1);

                //Fade in la main
                handClick.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true);
                handClick.transform.Find("Right").GetComponent<Image>().enabled = false;

                //Flash le background
                handClick.DOFade(0.15f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

                modules.delayedAction.Do(0.75f,
                    delegate ()
                    {
                        modules.proxyButton.ProxyScreen(onComplete);
                    });
            }
            else
            {
                modules.spotlight.FillCenter(true, true);
                modules.spotlight.On();

                pcControls = Instantiate(keyboardControlsPrefab.gameObject, modules.transform).GetComponent<CanvasGroup>();

                pcControls.alpha = 0;
                pcControls.DOFade(1, 0.6f).SetUpdate(true);

                //turnRightOrLeft = Instantiate(turnRightOrLeftPrefab.gameObject, modules.transform).GetComponent<CanvasGroupBehaviour>();
                //turnRightOrLeft.transform.localScale = Vector3.one;
                //turnRightOrLeft.HideInstant();
                //turnRightOrLeft.Show();

                //RectTransform tr = turnRightOrLeft.GetComponent<RectTransform>();
                //tr.anchorMin = new Vector2(0.5f, 0.5f);
                //tr.anchorMax = tr.anchorMin;


                modules.textDisplay.DisplayText("Turn left: A or LEFT ARROW.\n\nTurn right: D or RIGHT ARROW", true);

                modules.delayedAction.Do(0.75f,
                    delegate ()
                    {
                        modules.waitForInput.OnAnyKeyDown(onComplete);
                    });
            }
        }

        void TutoMove2(Image handClick, CanvasGroupBehaviour turnRightOrLeft, CanvasGroup handheld, CanvasGroup pcControls)
        {
            modules.shorcuts.TimeFreeze();
            modules.textDisplay.SetBottom();

            // On reflip le handheld du bon coté
            if (handheld != null)
                handheld.transform.localScale = Vector3.one;

            Action onComplete = () =>
            {
                if (handClick != null)
                {
                    CanvasGroup cg = handClick.GetComponent<CanvasGroup>();
                    cg.DOKill();
                    cg.DOFade(0, 0.5f).SetUpdate(true).OnComplete(cg.DestroyGO);
                }
                if (handheld != null)
                {
                    handheld.DOKill();
                    handheld.DOFade(0, 0.5f).SetUpdate(true).onComplete = handheld.DestroyGO;
                }

                if (turnRightOrLeft != null)
                    turnRightOrLeft.Hide();

                modules.shorcuts.TimeUnFreeze();
                modules.textDisplay.HideText();
                modules.spotlight.Off();
            };

            if (isMobile)
            {
                handClick.transform.Find("Right").GetComponent<Image>().enabled = true;
                handClick.transform.Find("Left").GetComponent<Image>().enabled = false;
                handClick.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetUpdate(true);

                modules.textDisplay.DisplayText("Turn right by clicking on the right side of your screen", true);


                RectTransform tr = handClick.GetComponent<RectTransform>();
                tr.anchorMin = new Vector2(0.6667f, 0);
                tr.anchorMax = new Vector2(1, 1);
                tr.anchoredPosition = Vector2.zero;
                tr.sizeDelta = Vector2.zero;

                modules.delayedAction.Do(0.75f,
                    delegate ()
                    {
                        modules.proxyButton.ProxyScreen(onComplete);
                    });
            }
            else
            {
                if (pcControls != null)
                    pcControls.DOFade(0, 0.6f).SetUpdate(true).onComplete = pcControls.DestroyGO;

                onComplete();
                //modules.spotlight.On();

                //turnRightOrLeft.Show();
                //turnRightOrLeft.transform.GetChild(0).gameObject.SetActive(false);
                //turnRightOrLeft.transform.GetChild(1).gameObject.SetActive(true);

                //modules.textDisplay.DisplayText("Turn right by pressing D or RIGHT ARROW", true);

                //modules.waitForInput.OnKeyDown(onComplete, KeyCode.D, KeyCode.RightArrow);
            }
        }

        void TutoEnemy(Unit unit)
        {
            if (!(unit is EnemyVehicle))
                return;

            Action onComplete = () =>
            {
                modules.shorcuts.TimeUnFreeze();
                InitQueue queue = new InitQueue(End);
                modules.spotlight.Off(queue.RegisterTween());
                modules.textDisplay.HideText(queue.RegisterTween());
                queue.MarkEnd();
            };

            modules.delayedAction.Do(1.5f, delegate ()
            {
                modules.shorcuts.TimeFreeze();
                modules.spotlight.FillCenter(false, false);
                if (unit != null)
                    modules.spotlight.OnWorld(unit.Position);
                else
                    modules.spotlight.OnWorld(Game.Instance.Player.vehicle.Position);

                modules.textDisplay.DisplayText("Defeat your enemies by striking them."
                    + " But be careful! They will try to attack you too!", true);

                modules.delayedAction.Do(0.75f,
                    delegate ()
                    {
                        if (isMobile)
                            modules.proxyButton.ProxyScreen(delegate ()
                            {
                                modules.spotlight.On(Game.Instance.ui.itemsDisplay.transform.position);

                                modules.textDisplay.DisplayText("Here are displayed your items."
                                    + " If you get hit, you will lose one item. If you have none to spare, you will die. They act like health points.", true);

                                modules.delayedAction.Do(0.75f, delegate ()
                                {
                                    if (isMobile)
                                        modules.proxyButton.ProxyScreen(onComplete);
                                    else
                                        modules.waitForInput.OnAnyKeyDown(onComplete);
                                });

                            });
                        else
                            modules.waitForInput.OnAnyKeyDown(delegate ()
                            {
                                modules.spotlight.On(Game.Instance.ui.itemsDisplay.transform.position);

                                modules.textDisplay.DisplayText("Here are displayed your items."
                                    + " If you get hit, you will lose one item. If you have none to spare, you will die. They act like health points.", true);

                                modules.delayedAction.Do(0.75f, delegate ()
                                {
                                    if (isMobile)
                                        modules.proxyButton.ProxyScreen(onComplete);
                                    else
                                        modules.waitForInput.OnAnyKeyDown(onComplete);
                                });
                            });
                    });
            });

            Game.Instance.onUnitSpawned -= TutoEnemy;
        }


    }

}