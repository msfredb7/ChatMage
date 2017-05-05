using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustment : MonoBehaviour
{
    void Start()
    {
        Vector2 bounds = Game.instance.ScreenBounds;
        transform.position = new Vector3(
            bounds.x / 2,               //x
            bounds.y / 2,               //y
            -100);                      //z
    }
}
