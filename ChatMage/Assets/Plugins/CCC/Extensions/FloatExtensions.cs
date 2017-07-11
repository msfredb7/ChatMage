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
}
