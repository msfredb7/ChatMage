using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;

using UnityEngine.Events;

public class Fred_TestScript : MonoBehaviour
{
    public MonoBehaviour someOtherScript;

    void Start()
    {
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameSaves.instance.SetInt(GameSaves.Type.Levels, "test", 3);
            GameSaves.instance.SetInt(GameSaves.Type.Levels, "teswdt", 66);
            GameSaves.instance.SetString(GameSaves.Type.Levels, "teswdt", "dwada");
            GameSaves.instance.SetObjectClone(GameSaves.Type.Levels, "teswdt", "dwada");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameSaves.instance.SetFloat(GameSaves.Type.Levels, "pogo", 3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameSaves.instance.DeleteFloat(GameSaves.Type.Levels, "pogo");
        }
    }
}