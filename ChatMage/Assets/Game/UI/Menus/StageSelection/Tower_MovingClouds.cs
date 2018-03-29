using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_MovingClouds : MonoBehaviour
{
    [SerializeField] SpriteOffset spriteOffset;
    [SerializeField] Vector2 speed;
    void Update()
    {
        spriteOffset.Offset += speed * Time.deltaTime;
    }
}
