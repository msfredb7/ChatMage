using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toucher
{
    public static bool IsTouching
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#elif UNITY_ANDROID
            return Input.touchCount >= 1;
#endif
        }
    }

    public static Vector2 GetTouchPosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#elif UNITY_ANDROID
        return Input.GetTouch(0).position;
#endif
    }
}
