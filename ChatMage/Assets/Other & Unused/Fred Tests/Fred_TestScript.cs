using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;

public class Fred_TestScript : MonoBehaviour
{
    public LaserSword sword;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sword.Open(()=>print("top"));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            sword.Close(() => print("bottom"));
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            sword.OpenInstant();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            sword.CloseInstant();
        }
    }
}