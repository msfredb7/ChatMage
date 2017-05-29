﻿using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSmash : PlayerComponent
{
    public event SimpleEvent onSmashGained;
    public event SimpleEvent onSmashUsed;

    public bool SmashEquipped { get { return smash != null; } }

    public bool HasSmash { get { return hasSmash; } }
    private bool hasSmash;

    [System.NonSerialized]
    private Smash smash;

    public override void OnGameReady()
    {
        if (smash != null)
            smash.OnGameReady();
    }

    public override void OnGameStarted()
    {
        if (smash != null)
            smash.OnGameStarted();
    }

    public void SetSmash(Smash smash)
    {
        this.smash = smash;
        if (smash != null)
            smash.Init(controller);
    }

    //Smash gained !
    public void GainSmash()
    {
        hasSmash = true;
        if (onSmashGained != null)
            onSmashGained();
    }

    //Utilisation du smash !
    public void SmashClick()
    {
        if (!hasSmash || smash == null)
            return;
        hasSmash = false;

        smash.OnSmash();
        if (onSmashUsed != null)
            onSmashUsed.Invoke();
    }
}
