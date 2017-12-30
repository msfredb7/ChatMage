using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;
using UnityEngine.Events;

public class Fred_TestScript : MonoBehaviour
{
    public int nextClient = 0;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");

        print(10.GetLeftmostSetBit());
        print(9.GetLeftmostSetBit());
        print(8.GetLeftmostSetBit());
        print(178.GetLeftmostSetBit());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print(ITM_DarkMoleSword.DivideAlgo(nextClient, 90));
            nextClient++;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }
}