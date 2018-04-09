using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemsDisplay_FlyingIntro : MonoBehaviour
{
    [Header("Start"), SerializeField] private RectTransform initialBottomPosition;
    [Header("Start"), SerializeField] private RectTransform initialTopPosition;

    [Header("Rotate Up"), SerializeField] private float initialRotation = -45;
    [SerializeField] private float rotateUpValue = 0;
    [SerializeField] private Ease rotateUpEase = 0;

    [Header("Scale Up"), SerializeField] private float scaleUpValue = 1.5f;
    [SerializeField] private float scaleUpDuration = 1;
    [SerializeField] private Ease scaleUpEase = Ease.OutBack;

    [Header("Move"), SerializeField] private float moveDelay = 1;
    [SerializeField] private float moveDuration = 1;
    [SerializeField] private Ease moveEase = Ease.Linear;

    [Header("Rotate Down"), SerializeField] private float rotateDownValue = -45;

    [Header("Scale Down"), SerializeField] private float scaleDownValue = 1;
    [SerializeField] private float scaleDownDuration = 1;
    [SerializeField] private Ease scaleDownEase = Ease.Linear;

    [Header("End"), SerializeField] private RectTransform finalPosition;
    [Header("End"), SerializeField] private RectTransform finalFullPosition;

    private class Animation
    {
        public Sequence sequence;
        public Coroutine coroutine;
    }

    private List<Animation> onGoingAnimations = new List<ItemsDisplay_FlyingIntro.Animation>();

    public void HandleBallIntro(ItemsDiplay_Ball ball, Vector3 spawnPosition, bool goToFullPosition, Action onAlmostComplete, Action onComplete)
    {
        Animation newAnim = new Animation()
        {
            sequence = DOTween.Sequence().SetId(ball),
            coroutine = null,
        };

        onGoingAnimations.Add(newAnim);

        newAnim.coroutine = StartCoroutine(AnimationRoutine(ball, spawnPosition, goToFullPosition, onAlmostComplete, ()=>
        {
            onGoingAnimations.Remove(newAnim);
            onComplete();
        }, newAnim.sequence));
    }

    public bool Interrupt(ItemsDiplay_Ball ball)
    {
        for (int i = 0; i < onGoingAnimations.Count; i++)
        {
            if(onGoingAnimations[i].sequence.id == (object)ball)
            {
                onGoingAnimations[i].sequence.Kill();
                StopCoroutine(onGoingAnimations[i].coroutine);
                onGoingAnimations.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    IEnumerator AnimationRoutine(ItemsDiplay_Ball ball, Vector3 spawnPosition, bool goToFullPosition, Action onAlmostComplete, Action onComplete,  Sequence sequence)
    {
        Transform tr = ball.transform;

        //Disable gravity component
        ball.GetGravityComponent().enabled = false;

        //Set ball to initial position
        tr.position = spawnPosition;//useTopSpawn ? initialTopPosition.position :  initialBottomPosition.position;

        //Scale up
        tr.localScale = Vector3.zero;
        sequence.Join(tr.DOScale(scaleUpValue, scaleUpDuration).SetEase(scaleUpEase));

        //Rotate up
        tr.rotation = Quaternion.Euler(Vector3.forward * initialRotation);
        sequence.Join(tr.DORotate(Vector3.forward * rotateUpValue, scaleUpDuration).SetEase(rotateUpEase));

        //Pause
        sequence.AppendInterval(moveDelay);

        //Scale down
        sequence.Append(tr.DOScale(scaleDownValue, scaleDownDuration).SetEase(scaleDownEase));

        //Move
        var finalPos = goToFullPosition ? finalFullPosition.position : finalPosition.position;
        sequence.Join(tr.DOMove(finalPos, moveDuration).SetEase(moveEase));

        //Rotate down
        var rotateDownDuration = moveDuration / 2;
        sequence.Join(tr.DORotate(Vector3.forward * rotateDownValue, rotateDownDuration).SetEase(Ease.OutSine));

        //Rotate down p2
        sequence.Insert(sequence.Duration() - rotateDownDuration, tr.DORotate(Vector3.forward * rotateUpValue, rotateDownDuration).SetEase(Ease.InSine));


        yield return sequence.WaitForPosition(sequence.Duration() - 0.35f);
        onAlmostComplete();
        yield return sequence.WaitForCompletion();
        onComplete();
    }
}
