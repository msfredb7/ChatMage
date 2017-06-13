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

    void Start()
    {
        /*
        print("(3 % 5) = " + (3 % 5));
        print("(4 % 5) = " + (4 % 5));
        print("(5 % 5) = " + (5 % 5));
        print("(6 % 5) = " + (6 % 5));
        print("(10 % 5) = " + (10 % 5));
        */
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            map.MoveTo(0, 2, true, LevelSelect.LevelSelect_MapAnimator.MoveToType.LeftSide);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            map.MoveTo(2000, true, LevelSelect.LevelSelect_MapAnimator.MoveToType.Centered);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            map.MoveTo(2000, true, LevelSelect.LevelSelect_MapAnimator.MoveToType.LeftSide);
        }
    }
}
