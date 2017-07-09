using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    public Vector2 v;
    public Vector2 min;
    public Vector2 max;

    void Start()
    {
        v = v.Clamped(min, max);
        print(v);
    }
}