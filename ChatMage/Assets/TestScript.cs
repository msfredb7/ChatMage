using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    public SpriteOffset sprOffset;
    public float duration = 2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sprOffset.DOOffset(Vector2.one, duration).SetLoops(-1);
        }
    }
}