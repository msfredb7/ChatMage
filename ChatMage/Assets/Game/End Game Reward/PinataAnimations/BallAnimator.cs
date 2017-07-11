using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EndGameReward
{
    public class BallAnimator : MonoBehaviour
    {
        public Animator myAnimator;
        public Transform ballHole;

        private float currentSpeed = 4;

        void Awake()
        {
            myAnimator.SetFloat("speed", currentSpeed);
            //myAnimator.set
        }

        void Start()
        {
        }

        public void Stop(float duration = 3)
        {
            DOTween.To(() => currentSpeed, (x) => { currentSpeed = x; myAnimator.SetFloat("speed", currentSpeed); }, 0, duration)
                .SetEase(Ease.InSine)
                .SetUpdate(false);
        }

        public Vector2 GetBallHole()
        {
            throw new System.Exception("a faire");
            //return ballHole.position;
        }
    }
}
