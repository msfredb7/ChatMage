using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class SM_AC130 : Smash
{
    public AC130Effect effectPrefab;
    public float duration = 10;

    [fsIgnore, NonSerialized]
    private AC130Effect effect;

    public override void OnGameReady()
    {
        effect = Instantiate(effectPrefab.gameObject).GetComponent<AC130Effect>();
    }

    public override void OnGameStarted()
    {
    }

    public override void OnSmash()
    {
        effect.Smash(duration, OnSmashEnd);
    }

    private void OnSmashEnd()
    {
    }

    public override void OnUpdate()
    {
    }
}
