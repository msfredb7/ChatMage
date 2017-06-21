using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_InstaKill : Smash
{
    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
    }

    public override void OnSmash(Action onComplete)
    {
        Debug.Log("insta kill smash !");
        if (onComplete != null)
            onComplete();
    }

    public override void OnUpdate()
    {
    }
}
