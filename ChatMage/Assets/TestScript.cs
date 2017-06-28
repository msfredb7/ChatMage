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
    public Transform trs;
    public float something;

    void Start()
    {
        //Vector2.
        //print(Vector2.right * Vector2.up);
    }

    void Update()
    {
        //print("Area: " + CCC.Math.AreaWithin.GetAreaWithin(TrToV()));
        //print("Resemblance: " + CCC.Math.AreaWithin.ResemblanceToCircle(2.5f, TrToV()));
    }

    //Vector2[] TrToV()
    //{
    //    Vector2[] vs = new Vector2[trs.childCount];
    //    for (int i = 0; i < vs.Length; i++)
    //    {
    //        vs[i] = trs.GetChild(i).position;
    //    }
    //    return vs;
    //}
}