using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TUT_SecondLevel : BaseTutorial
    {
        public CanvasGroup handClickPrefab;

        protected override void Cleanup()
        {
        }

        protected override void OnStart()
        {
            Game.instance.smashManager.onSmashSpawned += delegate ()
            {
                modules.delayedAction.Do(1f, FocusOnSmash);
                modules.delayedAction.Do(7.5f, DeFocusOnSmash);
            };
        }

        public void FocusOnSmash()
        {
            modules.shorcuts.TimeFreeze();

            // Parfois la smashball est en train de flasher parce qu'on la hit quand le tutoriel start
            if (!Game.instance.smashManager.CurrentSmashBall.GetComponent<SmashBallAnimator>().IsVisible())
                Game.instance.smashManager.CurrentSmashBall.GetComponent<SmashBallAnimator>().SetVisible(true);

            // Quand la smashball va mourrir faudra faire l'autre event
            Game.instance.smashManager.CurrentSmashBall.onDeath += FocusOnSmashInput;

            modules.spotlight.OnWorld(Game.instance.smashManager.CurrentSmashBall.Position, delegate () {
                modules.textDisplay.SetBottom();
                modules.textDisplay.automaticallyAdjustSize = true;
                modules.textDisplay.DisplayText("This is a SMASHBALL. " +
                    "You can hit it 3 times to break it and gain an incredible power." +
                    "This power can only be use once until the next SMASHBALL so use it at the right moment!", true);
            });
        }

        public void DeFocusOnSmash()
        {
            modules.textDisplay.HideText(delegate ()
            {
                modules.spotlight.Off(delegate() { modules.shorcuts.TimeUnFreeze(); });
            });
        }

        public void FocusOnSmashInput(Unit unit)
        {
            modules.shorcuts.TimeFreeze();
            modules.textDisplay.SetBottom();
            modules.textDisplay.DisplayText("Use your Smash by hitting the middle of the screen.", true);

            Image handClick = Instantiate(handClickPrefab.gameObject, modules.transform).GetComponent<Image>();

            //Fade in la main
            handClick.GetComponent<CanvasGroup>().DOFade(1, 1).SetUpdate(true);

            RectTransform tr = handClick.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0.333f, 0);
            tr.anchorMax = new Vector2(0.666f, 1);
            tr.anchoredPosition = Vector2.zero;
            tr.sizeDelta = Vector2.zero;

            //Flash le background
            handClick.DOFade(0.15f, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

            modules.delayedAction.Do(0.75f,
                delegate ()
                {
                    modules.proxyButton.ProxyScreen(
                        delegate ()
                        {
                            Destroy(handClick.gameObject);
                            modules.shorcuts.TimeUnFreeze();
                            modules.textDisplay.HideText();
                            End(true);
                        });
                });
        }
    }
}

