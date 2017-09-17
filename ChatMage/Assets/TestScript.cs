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
    public SpriteOffset sprOffset;
    public float duration = 2;

    void Update()
    {
        float rotateFloat = (((-5 + 1) * 60) - 30) - transform.rotation.z;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, rotateFloat));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T))
        {

        }
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
    }
}