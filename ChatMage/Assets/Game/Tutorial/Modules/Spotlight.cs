using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tutorial
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Spotlight : MonoBehaviour
    {
        [Header("Fade in")]
        public float maxAlpha = 0.5f;
        public float fadeInDuration;
        public Ease fadeInEase = Ease.Linear;

        [Header("Fade out")]
        public float fadeOutDuration;
        public Ease fadeOutEase = Ease.Linear;

        [Header("Move")]
        public float moveDuration;
        public Ease moveEase = Ease.InOutSine;


        private CanvasGroup group;
        private Tweener fadeTween;
        private Tweener moveTween;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            InstantOff();
        }

        public void InstantOff()
        {
            group.alpha = 0;
        }

        public void Off(TweenCallback onComplete = null)
        {
            if (fadeTween != null)
                fadeTween.Kill();
            fadeTween = group.DOFade(0, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true);

            if (onComplete != null)
                fadeTween.OnComplete(onComplete);
        }

        public void On(TweenCallback onComplete = null)
        {
            if (fadeTween != null)
                fadeTween.Kill();
            fadeTween = group.DOFade(maxAlpha, fadeInDuration).SetEase(fadeInEase).SetUpdate(true);

            if (onComplete != null)
                fadeTween.OnComplete(onComplete);
        }

        public void On(Vector2 absolutePosition, TweenCallback onComplete = null)
        {
            if (moveTween != null)
                moveTween.Kill();
            moveTween = GetComponent<RectTransform>().DOMove(absolutePosition, moveDuration).SetEase(moveEase).SetUpdate(true);
            On(onComplete);
        }
    }
}
