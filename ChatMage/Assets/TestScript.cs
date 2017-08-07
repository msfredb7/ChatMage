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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Camera cam = Game.instance.gameCamera.cam;
            cam.aspect = 16f / 9f;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {

        }
    }
}