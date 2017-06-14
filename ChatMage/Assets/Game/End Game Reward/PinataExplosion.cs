using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EndGameReward
{
    public class PinataExplosion : FullInspector.BaseBehavior
    {
        public CanvasGroup group;
        public RectTransform toWiggle;


        protected override void Awake()
        {
            base.Awake();
            group.alpha = 0;
        }

        public void SetCenter(Vector2 center)
        {

        }

        [FullInspector.InspectorButton]
        public void Animate()
        {
            group.DOFade(1, 1);
            toWiggle.DOAnchorPosY(toWiggle.anchoredPosition.y + 300, 3)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            toWiggle.DORotate(Vector3.forward * 359.999f, 3, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}
