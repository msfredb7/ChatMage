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
#else
        return Input.GetMouseButton(0);
#endif
        }
    }

    public static bool GetTouchDown()
    {
        if (Application.isMobilePlatform)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                    return true;
            }
            return false;
        }
        else
            return Input.GetMouseButtonDown(0);
    }

    public static Vector2 GetTouchPosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#elif UNITY_ANDROID
        return Input.GetTouch(0).position;
#else
        return Input.mousePosition;
#endif
    }
}
