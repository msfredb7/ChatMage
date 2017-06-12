using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : BaseBehavior
{
    public LevelSelect.LevelSelect_MapAnimator map;
    public ScrollRect sr;
    public Mapping mapping;

    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            List<Waypoint> test = mapping.GetWaypoints("wda");
            if (test == null)
                print("null");
            else
                print(test.Count);
        }
    }
}
