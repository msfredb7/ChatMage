using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CCC.Utility;

public class InGameTweener : InGameTimescaleListener
{
    protected Tween t;

    protected override void UpdateTimescale(float worldTimescale)
    {
        if (t != null)
            t.timeScale = worldTimescale;
    }
}
