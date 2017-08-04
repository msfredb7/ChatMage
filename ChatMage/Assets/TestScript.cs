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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Armory.UnlockAccessToItems();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Armory.UnlockAccessToSmash();
        }
    }
}