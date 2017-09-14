using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using DG.Tweening;
using FullInspector;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<SpriteOffset>().DOOffset(Vector2.one, 4).SetLoops(-1);
        }
    }
}