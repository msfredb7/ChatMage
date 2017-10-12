using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_1_3_Gate : InGameTweener
{
    [Header("Animation")]
    public float duration;
    public Ease closeEase = Ease.OutBounce;
    public Ease openEase = Ease.InSine;
    public float overshoot = 1;

    [Header("Left")]
    public Transform leftPart;
    public Vector2 leftEnd;

    [Header("Right")]
    public Transform rightPart;
    public Vector2 rightEnd;

    private Vector2 leftStart;
    private Vector2 rightStart;
    private Sequence sq;

    void Awake()
    {
        leftStart = leftPart.localPosition;
        rightStart = rightPart.localPosition;

        leftPart.localPosition = leftEnd;
        rightPart.localPosition = rightEnd;
    }

    public void Open()
    {
        Open(null);
    }
    public void Close()
    {
        Close(null);
    }

    public void Open(TweenCallback onComplete)
    {
        PreAnimate();

        sq.Join(leftPart.DOLocalMove(leftEnd, duration).SetEase(openEase, overshoot));
        sq.Join(rightPart.DOLocalMove(rightEnd, duration).SetEase(openEase, overshoot));

        AddTimescaleListener();
        AddOnComplete(onComplete);
    }
    public void Close(TweenCallback onComplete)
    {
        PreAnimate();

        sq.Join(leftPart.DOLocalMove(leftStart, duration).SetEase(closeEase));
        sq.Join(rightPart.DOLocalMove(rightStart, duration).SetEase(closeEase));

        //On ne met PAS de timescale listener. On ne veut pas que le joueur d�passe la fermeture de la grille
        //AddTimescaleListener();
        AddOnComplete(onComplete);
    }

    private void PreAnimate()
    {
        if (sq == null)
        {
            sq = DOTween.Sequence();
            sq.SetUpdate(true);
            t = sq;
        }
        else
        {
            if (sq.IsActive())
            {
                sq.Kill();
            }

            sq = null;
            PreAnimate();
        }
    }
    private void AddOnComplete(TweenCallback onComplete)
    {
        if (onComplete != null)
            sq.OnComplete(onComplete);
    }
}
