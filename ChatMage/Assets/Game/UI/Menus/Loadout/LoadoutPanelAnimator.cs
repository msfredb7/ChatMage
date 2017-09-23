using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadoutPanelAnimator : MonoBehaviour
{
    public float moveDuration = 0.5f;
    public Ease exitEase = Ease.InSine;
    public Ease enterEase = Ease.OutSine;

    RectTransform tr;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
    }

    public void ShowInstant()
    {
        tr.anchoredPosition = new Vector2(0, tr.anchoredPosition.y);
    }

    public void ExitLeft(TweenCallback onComplete)
    {
        Goto(0, -1920, exitEase, onComplete);
    }

    public void ExitRight(TweenCallback onComplete)
    {
        Goto(0, 1920, exitEase, onComplete);
    }

    public void EnterLeft(TweenCallback onComplete)
    {
        Goto(-1920, 0, enterEase, onComplete);
    }

    public void EnterRight(TweenCallback onComplete)
    {
        Goto(1920, 0, enterEase, onComplete);
    }

    private void Goto(float from, float to, Ease ease, TweenCallback onComplete)
    {
        tr.DOKill();

        //Position initial
        tr.anchoredPosition = new Vector2(from, tr.anchoredPosition.y);

        //Move
        Tween tw = tr.DOAnchorPosX(to, moveDuration).SetEase(ease);

        //Callback
        if (onComplete != null)
            tw.OnComplete(onComplete);
    }
}
