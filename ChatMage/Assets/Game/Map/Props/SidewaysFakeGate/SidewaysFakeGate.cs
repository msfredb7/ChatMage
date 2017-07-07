using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SidewaysFakeGate : MonoBehaviour
{
    [Header("Settings")]
    public Transform rendering;
    public Vector2 openedLocalPosition;
    public bool openedOnStart = true;

    [Header("Animation")]
    public float animDuration = 0.7f;
    public Ease ease;

    private Vector2 closedLocalPosition;
    private Tweener moveTween;

    void Awake()
    {
        closedLocalPosition = rendering.localPosition;

        if (openedOnStart)
            InstantOpen();
        else
            InstantClose();
    }

    public void Close()
    {
        if (moveTween != null)
            moveTween.Kill();
        moveTween = rendering.DOLocalMove(closedLocalPosition, animDuration).SetEase(ease);
    }

    public void Open()
    {
        if (moveTween != null)
            moveTween.Kill();
        moveTween = rendering.DOLocalMove(openedLocalPosition, animDuration).SetEase(ease);
    }

    public void InstantOpen()
    {
        rendering.localPosition = openedLocalPosition;
    }

    public void InstantClose()
    {
        rendering.localPosition = closedLocalPosition;
    }
}
