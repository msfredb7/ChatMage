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
    public ZaWarudoEffect effect;
    private bool animating = false;
    void Start()
    {
        MasterManager.Sync();
    }
    void Update()
    {
        //if ((Input.GetMouseButtonDown(0) || Input.touchCount != 0) && !animating)
        //{
        //    animating = true;
        //    effect.Animate(null);
        //    DelayManager.CallTo(delegate () { effect.AnimateBack(null); }, 5);
        //    DelayManager.CallTo(delegate () { animating = false; }, 7.5f);
        //}
    }
}
