using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class InGameAnimator : InGameTimescaleListener
{
    public Animator controller;

    protected override void UpdateTimescale(float worldTimescale)
    {
        controller.speed = worldTimescale;
    }
}
