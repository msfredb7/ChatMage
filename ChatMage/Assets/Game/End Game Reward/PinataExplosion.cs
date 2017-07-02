using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FullInspector;

namespace EndGameReward
{
    public class PinataExplosion : BaseBehavior
    {
        public enum BallColor { Blue = 0, Red = 1 }

        [InspectorHeader("Linking")]
        public Transform explosionCenter;
        public SpriteRenderer whiteForeground;

        [InspectorHeader("Camera")]
        public Transform cam;
        public VectorShaker vectorShaker;

        [InspectorHeader("Flier")]
        public Transform flier;
        public Vector2 flierFinalPos;

        [InspectorHeader("Star 1")]
        public Ease starGrowEase;
        public SpriteRenderer starOne;
        public float starOneFinalSize;

        [InspectorHeader("Star 2")]
        public SpriteRenderer starTwo;
        public float starTwoFinalSize;

        [InspectorHeader("Star 3")]
        public SpriteRenderer starThree;
        public float starThreeFinalSize;

        [InspectorHeader("Light")]
        public SpriteRenderer lightFade;
        public float lightFinalSize;

        [InspectorHeader("Ball")]
        public BallAnimator blueBall;
        public BallAnimator redBall;
        public float ballFinalSize = 1.25f;

        private BallAnimator theBall;

        [InspectorHeader("Confetti 1")]
        public Transform confettiOne;
        public float confettiOneFinalSize = 1.25f;
        public Vector2 confettiOneFinalPos;

        [InspectorHeader("Confetti 2")]
        public Transform confettiTwo;
        public float confettiTwoFinalSize = 1.25f;
        public Vector2 confettiTwoFinalPos;

        [InspectorHeader("Other")]
        public MovingSprite[] movingSprites;

        private bool animating = false;
        private Vector3 baseCameraPosition;


        protected override void Awake()
        {
            base.Awake();
            explosionCenter.gameObject.SetActive(false);
            //group.alpha = 0;
        }

        void Update()
        {
            if (!animating)
                return;

            cam.position = baseCameraPosition + (Vector3)vectorShaker.CurrentVector;
        }

        public void Animate(Vector2 explosionCenter, BallColor ballColor)
        {
            transform.position = cam.position + Vector3.forward;
            this.explosionCenter.position = new Vector3(explosionCenter.x, explosionCenter.y, transform.position.z);

            Animate(ballColor);
        }
        
        private void Animate(BallColor ballColor)
        {
            animating = true;
            baseCameraPosition = cam.position;
            vectorShaker.Shake(1, 0.25f);

            explosionCenter.gameObject.SetActive(true);

            whiteForeground.color = Color.white;
            whiteForeground.DOFade(0, 1);

            flier.DOMove(new Vector3(cam.position.x + flierFinalPos.x, cam.position.y + flierFinalPos.y, flier.position.z), 4);

            starOne.transform.DOScale(starOneFinalSize, 4).SetEase(starGrowEase);
            starTwo.transform.DOScale(starTwoFinalSize, 4).SetEase(starGrowEase);
            starThree.transform.DOScale(starThreeFinalSize, 4).SetEase(starGrowEase);

            starOne.transform.DORotate(Vector3.back * 360, 80, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            starTwo.transform.DORotate(Vector3.forward * 360, 80, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            starThree.transform.DORotate(Vector3.back * 360, 140, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

            lightFade.transform.DOScale(lightFinalSize, 4);



            //C'est important que les balles commence en etant 'active' et qu'on desactive celles non-utilisers ensuite
            // ET NON L'INVERSE. Ca creer des gros spike de cpu sinon.
            switch (ballColor)
            {
                case BallColor.Blue:
                    redBall.gameObject.SetActive(false);
                    theBall = blueBall;
                    break;
                case BallColor.Red:
                    blueBall.gameObject.SetActive(false);
                    theBall = redBall;
                    break;
            }
            theBall.gameObject.SetActive(true);
            theBall.transform.DOScale(ballFinalSize, 4).SetEase(Ease.OutSine);
            theBall.Stop(4);

            confettiOne.DOScale(confettiOneFinalSize, 5).SetEase(Ease.OutSine);
            confettiOne.DOLocalMove(confettiOneFinalPos, 5).SetEase(Ease.OutSine);

            confettiTwo.DOScale(confettiTwoFinalSize, 6).SetEase(Ease.OutSine);
            confettiTwo.DOLocalMove(confettiTwoFinalPos, 6).SetEase(Ease.OutSine);

            Sequence sq = DOTween.Sequence();

            for (int i = 0; i < movingSprites.Length; i++)
            {
                movingSprites[i].Launch(sq);
            }
        }

        public class MovingSprite
        {
            public Ease easing = Ease.OutSine;
            public Transform tr;
            public Vector2 finalPos;
            public Vector3 finalScale = Vector3.one;
            public float duration = 4;
            public float startAt = 0;

            public void Launch(Sequence sequence)
            {
                sequence.InsertCallback(startAt, delegate () { tr.GetComponent<SpriteRenderer>().enabled = true; });

                sequence.Insert(startAt, tr.DOLocalMove(finalPos, duration).SetEase(easing));
                sequence.Insert(startAt, tr.DOScale(finalScale, duration).SetEase(easing));
            }
        }
    }
}
