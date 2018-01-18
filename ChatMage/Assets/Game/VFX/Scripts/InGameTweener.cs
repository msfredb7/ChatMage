using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CCC.Utility;

public class InGameTweener : InGameTimescaleListener
{
    protected Tween tween;

    protected override void UpdateTimescale(float worldTimescale)
    {
        if (tween != null)
            tween.timeScale = worldTimescale;
    }

    protected void KillTween()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
    }
}
