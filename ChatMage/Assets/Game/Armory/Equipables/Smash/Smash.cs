using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public abstract class Smash : Equipable
{
    public float minimumJuice = 3;
    public bool overrideDefaultMaxJuice = false;
    public float newMaxJuice = 10;
    public bool canGainJuiceWhileSmashing = false;
    public bool clearAllJuiceOnCompletion = true;

    public const float DEFAULT_MAX_JUICE = 5;

    public abstract void OnSmash(Action onComplete);
    public override void OnUpdate()
    {

    }

    public float GetMinJuice()
    {
        return minimumJuice;
    }
    public float GetMaxJuice()
    {
        return overrideDefaultMaxJuice ? newMaxJuice : DEFAULT_MAX_JUICE;
    }
}
