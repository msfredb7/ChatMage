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
