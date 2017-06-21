using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class SM_AC130 : Smash
{
    public AC130Effect effectPrefab;
    public float duration = 30;
    public int ammo = 6;

    [fsIgnore, NonSerialized]
    private AC130Effect effect;

    public override void OnGameReady()
    {
        effect = Instantiate(effectPrefab.gameObject, Game.instance.currentLevel.inGameEvents.transform).GetComponent<AC130Effect>();
    }

    public override void OnGameStarted()
    {
    }

    public override void OnSmash(Action onComplete)
    {
        effect.Smash(duration, ammo, onComplete);
    }

    public override void OnUpdate()
    {
    }
}
