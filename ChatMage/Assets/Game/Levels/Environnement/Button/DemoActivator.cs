using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoActivator : MonoBehaviour, IActivator
{
    public void Activate()
    {
        Debug.Log("Button Activated");
    }
}
