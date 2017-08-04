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

    void Awake()
    {
        Debug.LogWarning("Testscript here on " + gameObject.name + ". Don't forget me !");
    }

    public string lootboxRefName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Game.instance.gameCamera.maxHeight = Game.instance.gameCamera.Height;
            Game.instance.gameCamera.minHeight = Game.instance.gameCamera.Height;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Game.instance.map.roadPlayer.CurrentRoad.ApplyMinMaxToCamera();
        }
    }
}