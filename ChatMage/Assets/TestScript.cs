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
    public InGameEvents ingameEvents;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (smasher.CurrentSmashBall != null)
                smasher.CurrentSmashBall.ForceDeath();
            else
                smasher.DecreaseCooldown(30);
        }
    }

    void OnComplete()
    {

    }
}
