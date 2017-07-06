using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tutorial
{
    public class TextDisplay : MonoBehaviour
    {
        public Text text;
        public CanvasGroup blackFade;

        [Header("Fade settings")]
        public float fadeDuration;
        public Ease fadeEase;

        [Header("Size settings")]
        public bool automaticallyAdjustSize;
        public AnimationCurve textLengthToX;
        public AnimationCurve textLengthToY;

        [Header("Height settings")]
        public Vector2 topPosition;
        public Vector2 middlePosition;
        public Vector2 bottomPosition;

        private bool isOn = false;

        void Awake()
        {
            InstantHide();
        }

        public void SetBottom()
        {
            GetComponent<RectTransform>().anchoredPosition = bottomPosition;
        }

        public void SetMiddle()
        {
            GetComponent<RectTransform>().anchoredPosition = middlePosition;
        }

        public void SetTop()
        {
            GetComponent<RectTransform>().anchoredPosition = topPosition;
        }

        public void SetSize(int messageLength)
        {
            RectTransform tr = GetComponent<RectTransform>();

            tr.sizeDelta = new Vector2(textLengthToX.Evaluate(messageLength), textLengthToY.Evaluate(messageLength));
        }

        public void InstantDisplay(string message, bool blackBackground)
        {
            blackFade.gameObject.SetActive(true);
            blackFade.alpha = blackBackground ? 1 : 0;

            if (automaticallyAdjustSize)
                SetSize(message.Length);
            text.enabled = true;
            text.color = new Color(1, 1, 1, 1);
            text.text = message;

            isOn = true;
        }

        public void InstantHide()
        {
            blackFade.alpha = 0;
            blackFade.gameObject.SetActive(false);
            text.enabled = false;
            text.color = new Color(1, 1, 1, 0);

            isOn = false;
        }

        public void DisplayText(string message, bool blackBackground, TweenCallback onComplete = null)
        {
            if (isOn)
            {
                HideText(delegate ()
                {
                    DisplayText(message, blackBackground, onComplete);
                });
            }
            else
            {
                if (blackBackground)
                {
                    blackFade.gameObject.SetActive(true);
                    blackFade.DOKill();
                    blackFade.DOFade(1, fadeDuration).SetEase(fadeEase).SetUpdate(true);
                }

                if (automaticallyAdjustSize)
                    SetSize(message.Length);
                text.enabled = true;
                text.text = message;
                text.DOKill();
                Tweener textFade = text.DOFade(1, fadeDuration).SetEase(fadeEase).SetUpdate(true);

                isOn = true;

                if (onComplete != null)
                    textFade.OnComplete(onComplete);
            }

        }

        public void HideText(TweenCallback onComplete = null)
        {
            if (blackFade.alpha > 0)
            {
                blackFade.DOKill();
                blackFade.DOFade(0, fadeDuration).SetEase(fadeEase).SetUpdate(true);
            }

            text.DOKill();
            text.DOFade(0, fadeDuration).SetEase(fadeEase).SetUpdate(true).OnComplete(delegate ()
            {
                InstantHide();

                if (onComplete != null)
                    onComplete();
            });

        }
    }
}
