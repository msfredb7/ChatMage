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
            GameSaves.instance.SetInt(GameSaves.Type.Loadout, "pogo", 45);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            print(GameSaves.instance.GetInt(GameSaves.Type.Loadout, "pogo"));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameSaves.instance.SaveAll(delegate() { print("save completed"); });
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameSaves.instance.LoadAll(null);
        }
    }
}
