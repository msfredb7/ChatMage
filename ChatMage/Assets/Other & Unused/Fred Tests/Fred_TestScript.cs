﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;

using UnityEngine.Events;

public class Fred_TestScript : MonoBehaviour
{
    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(
            ()=>
            {
                Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
            });
    }

    void Update()
    {
    }
}