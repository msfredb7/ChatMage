
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.Persistence;

public class FPSDisplay : MonoPersistent
{
    public override void Init(Action onComplete)
    {
        onComplete();
    }
}
