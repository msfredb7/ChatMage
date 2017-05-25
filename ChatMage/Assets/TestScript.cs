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
    public SmashManager smasher;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            smasher.SHITNIGGA();
        }
    }
}
