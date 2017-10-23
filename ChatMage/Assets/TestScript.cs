using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        Debug.LogWarning("Test script qui traine ici, ne m'oublier pas. (" + gameObject.name + ")");

        MasterManager.Sync();
    }

    public SmashDisplayV2 smd;
    [Range(0, 1)]
    public float juice;
    [Range(0, 1)]
    public float marker;


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        smd.SetJuiceValue01(juice, true);
        smd.SetMarkerValue01(marker);
        //}
    }
}