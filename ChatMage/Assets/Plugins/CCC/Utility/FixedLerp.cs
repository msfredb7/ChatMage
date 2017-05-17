﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixes your lerp (based on 120 fps)
/// </summary>
public class FixedLerp
{
    public const float defaultFPS = 120;
    private static float FPS
    {
        get { return 1f / Time.deltaTime; }
    }
    private static float UnscaledFPS
    {
        get { return 1f / Time.unscaledDeltaTime; }
    }

    public static float Fix(float lerpAmount)
    {
        return Fix(lerpAmount, FPS);
    }

    public static float FixScaled(float lerpAmount, float timeScale)
    {
        return Fix(lerpAmount, FPS*timeScale);
    }

    public static float Fix(float lerpAmount, float customFPS)
    {
        return 1 - Mathf.Pow(1 - lerpAmount, defaultFPS / customFPS);
    }

    public static float UnscaledFix(float lerpAmount)
    {
        return Fix(lerpAmount, UnscaledFPS);
    }
}
