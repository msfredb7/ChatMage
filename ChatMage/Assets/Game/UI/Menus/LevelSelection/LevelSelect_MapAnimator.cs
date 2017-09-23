using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LevelSelect
{
    public class LevelSelect_MapAnimator : MonoBehaviour
    {
        public enum MoveToType { Centered, LeftSide, RightSide}
        [Header("Nous utilisons leur 'anchoredPosition.x'")]
        public LevelSelect_Region[] regions;
        [Header("Linking")]
        public RectTransform content;
        public Image raycastBlocker;
        public ScrollRect scrollRect;

        [Header("Settings")]
        public float scrollDuration = 0.35f;

        private Tween moveTween;

        public void SetLastUnlockedRegionIndex(int lastRegionIndex)
        {
            //Ceci correspond a: position ancré de gauche + longeur de la région
            float x = regions[lastRegionIndex].rectTransform.anchoredPosition.x + regions[lastRegionIndex].rectTransform.sizeDelta.x;

            content.sizeDelta = new Vector2(x, content.sizeDelta.y);
        }

        public void MoveTo(int regionIndex, int levelIndex, bool animated, MoveToType moveType)
        {
            MoveTo(regions[regionIndex].rectTransform.anchoredPosition.x +
                regions[regionIndex].levelItems[levelIndex].rectTransform.anchoredPosition.x,
                animated,
                moveType);
        }

        /// <summary>
        /// Se déplace vers la cible, 0: la gauche completement,  1920 et +: droite
        /// </summary>
        public void MoveTo(float target, bool animated, MoveToType moveType = MoveToType.Centered)
        {
            if (content.sizeDelta.x <= 1920)
            {
                MoveToNormalized(0, animated);
            }
            else
            {
                switch (moveType)
                {
                    case MoveToType.Centered: //Va au milieu du screen
                        target -= 1920 / 2;
                        break;
                    case MoveToType.LeftSide: //Va à 1/6e du screen
                        target -= 1920 / 6;
                        break;
                    case MoveToType.RightSide: //Va à 5/6e du screen
                        target -= 5 * 1920 / 6;
                        break;
                }

                float normalizedTarget = target / (content.sizeDelta.x - 1920);
                MoveToNormalized(normalizedTarget, animated);
            }
        }

        private void MoveToNormalized(float normalizedPos, bool animated)
        {
            normalizedPos = Mathf.Clamp(normalizedPos, 0, 1);

            //Kill la dernière animation + re-enable le dragging du BG
            if (moveTween != null)
            {
                moveTween.Kill();
                raycastBlocker.enabled = true;
            }

            if (animated)
            {
                //Disable le dragging du BG
                raycastBlocker.enabled = false;

                moveTween = DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                    (x) => scrollRect.horizontalNormalizedPosition = x,
                    normalizedPos,
                    scrollDuration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(delegate()
                    {
                        raycastBlocker.enabled = true;
                    });
            }
            else
                scrollRect.horizontalNormalizedPosition = normalizedPos;
        }
    }
}
