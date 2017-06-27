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
    public Transform[] trs;

    void Start()
    {
    }

    void Update()
    {
        print("Area: " +
            CCC.Math.AreaWithin.GetAreaWithin(TrToV(), true));
        //print("is Left: " +
        //    CCC.Math.AreaWithin.IsLeft(
        //        trs[0].position,
        //        trs[1].position,
        //        trs[2].position));
    }

    Vector2[] TrToV()
    {
        Vector2[] vs = new Vector2[trs.Length];
        for (int i = 0; i < vs.Length; i++)
        {
            vs[i] = trs[i].position;
        }
        return vs;
    }
}