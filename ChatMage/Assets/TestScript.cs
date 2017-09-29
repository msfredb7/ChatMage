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
    }


    public JesusChains chains;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            chains.Break();
        }
    }
}