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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DelayManager.LocalCallTo(delegate () { print("hello"); }, 3, this);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopAllCoroutines();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Game.instance.Player.vehicle.TimeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Game.instance.Player.vehicle.TimeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Game.instance.Player.vehicle.TimeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Game.instance.Player.vehicle.Bump(Vector2.right*5,0.1f, BumpMode.VelocityAdd);
        }
    }
}
