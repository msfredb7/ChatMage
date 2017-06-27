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

    void Start()
    {
        StatFloat st = new StatFloat(10, 0, 100, BoundMode.Cap);

        st.AddBuff("per", 20, BuffType.Percent);
        st.AddBuff("add", 3, BuffType.Flat);
        st.AddBuff("per2", 20, BuffType.Percent);
        print(st.ToString());
        st.RemoveBuff("per");
        print(st.ToString());
        st.RemoveBuff("per2");
        print(st.ToString());
        st.RemoveBuff("add");
        print(st.ToString());
    }
}