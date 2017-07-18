using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static Vector2 ToVector(this float value)
    {
        return CCC.Math.Vectors.AngleToVector(value);
    }

    public static float Abs(this float value)
    {
        return Mathf.Abs(value);
    }

    public static float Capped(this float value, float max)
    {
        return Mathf.Min(value, max);
    }

    public static float Floored(this float value, float min)
    {
        return Mathf.Max(value, min);
    }

    public static float MoveTowards(this float value, float target, float delta)
    {
        return Mathf.MoveTowards(value, target, delta);
    }

    public static float Mod(this float value, float modulo)
    {
        if (modulo <= 0)
            return 0;

        if (value < 0)
            value = 0;
        else
            return value % modulo;
        return value;
    }
}
