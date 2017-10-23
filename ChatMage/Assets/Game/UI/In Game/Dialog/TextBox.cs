using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dialoguing
{
    public class TextBox : MonoBehaviour
    {
        [Header("Box")]
        public RectTransform background;
        public float openDuration;
        public Ease openEase;
        public Ease closeEase;

        [Header("Text")]
        public Text text;
        public float writeSpeed = 2;
        public ScrambleMode scrambling = ScrambleMode.None;
        public SoundPlayer scramblingSound;

        private Tweener textTween;
        private Tweener backgroundTween;
        private string targetMessage = "";

        void Awake()
        {
            text.text = "";
        }

        public void DisplayMessage(string message)
        {
            text.text = "";

            if (textTween != null)
            {
                textTween.Kill();
                scramblingSound.SetPlayerActive(false);
            }

            targetMessage = message;

            float duration = targetMessage.Length / writeSpeed;

            textTween = text.DOText(targetMessage, duration, scrambleMode: scrambling)
                .SetUpdate(true);
            scramblingSound.SetPlayerActive(true);
            scramblingSound.PlaySound();
            textTween.OnComplete(delegate () { scramblingSound.SetPlayerActive(false); });
        }

        public bool IsAnimatingText()
        {
            return textTween != null && textTween.IsPlaying();
        }

        public void Open(TweenCallback onComplete = null)
        {
            Vector3 scale = background.localScale;
            background.localScale = new Vector3(0, scale.y, scale.z);

            if (backgroundTween != null)
                backgroundTween.Kill();

            backgroundTween = background.DOScaleX(1, openDuration)
                .SetEase(openEase)
                .SetUpdate(true)
                .OnComplete(delegate ()
                {
                    text.gameObject.SetActive(true);
                    if (onComplete != null)
                        onComplete();
                });
        }

        public void Close(TweenCallback onComplete = null)
        {
            text.gameObject.SetActive(false);

            if (backgroundTween != null)
                backgroundTween.Kill();

            backgroundTween = background.DOScaleX(0, openDuration)
                .SetEase(closeEase)
                .SetUpdate(true);

            if (onComplete != null)
                backgroundTween.OnComplete(onComplete);
        }

        public void SpeedUp()
        {
            if (textTween != null)
            {
                textTween.Kill();
                scramblingSound.SetPlayerActive(false);
            }

            text.text = targetMessage;
        }
    }
}
