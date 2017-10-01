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

    public PortalVFX portal;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            portal.Open();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            portal.Close();
        }
    }
}