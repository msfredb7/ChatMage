using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasGroupBehaviour : MonoBehaviour
{
    [Header("Canvas group")]
    public CanvasGroup canvasGroup;
    [SerializeField, ReadOnlyInPlayMode]
    private float fadeDuration;
    public Ease ease = Ease.Linear;

    public bool isIndependantUpdate = false;

    [SerializeField, ReadOnlyInPlayMode]
    private bool keepTheSameTween = false;

    private Tween keepTween;

    public virtual void Hide()
    {
        if (keepTheSameTween)
        {
            CheckLaunchTween(0);
            keepTween.PlayForward();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(0);
        }
    }

    public virtual void Show()
    {
        if (keepTheSameTween)
        {
            CheckLaunchTween(0);
            keepTween.PlayBackwards();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(1);
        }
    }

    public virtual void HideInstant()
    {
        if (keepTheSameTween)
        {
            if (keepTween != null && keepTween.IsActive())
            {
                keepTween.PlayForward();
                keepTween.Complete();
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
        else
        {
            KillIfActive();
            canvasGroup.alpha = 0;
        }
    }

    public virtual void ShowInstant()
    {
        if (keepTheSameTween)
        {
            if (keepTween != null && keepTween.IsActive())
            {
                keepTween.Goto(0);
                keepTween.PlayBackwards();
            }
            else
            {
                canvasGroup.alpha = 1;
            }
        }
        else
        {
            KillIfActive();
            canvasGroup.alpha = 1;
        }
    }

    protected virtual void OnDestroy()
    {
        KillIfActive();
    }

    private void CheckLaunchTween(float alpha)
    {
        if (keepTween == null || !keepTween.IsActive())
            keepTween = AdjustTween(canvasGroup.DOFade(alpha, fadeDuration));
    }

    private void KillIfActive()
    {
        if (keepTween != null && keepTween.IsActive())
            keepTween.Kill();
        keepTween = null;
    }

    private Tween AdjustTween(Tween t)
    {
        return t.SetEase(ease)
                .SetAutoKill(!keepTheSameTween)
                .SetUpdate(isIndependantUpdate);
    }
}
