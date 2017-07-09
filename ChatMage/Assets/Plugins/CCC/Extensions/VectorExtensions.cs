using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 Clamped(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y));
    }
    public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z));
    }
    public static Vector2 Rotate(this Vector2 v, float angle)
    {
        return v.RotateRad(angle * Mathf.Deg2Rad);
    }
    public static Vector2 RotateRad(this Vector2 v, float radians)
    {
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
