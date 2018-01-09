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
    public Transform cube;
    public InputAction action;
    public StringVariable v1;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");

    }

    void Update()
    {
        if (action.GetDown())
        {
            v1.Value += 10;
        }
        //if (action.Get())
        //{
        //    cube.transform.position += Vector3.right * Time.deltaTime;
        //}
        //if (action.GetUp())
        //{
        //    cube.transform.position += Vector3.left * 2;
        //}
    }
}