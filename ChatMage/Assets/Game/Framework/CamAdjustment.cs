using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustment : MonoBehaviour
{
    //NON UTILIS�
    public void Adjust(Vector2 bounds)
    {
        transform.position = new Vector3(
            bounds.x / 2,               //x
            bounds.y / 2,               //y
            -10);                      //z
    }
}
