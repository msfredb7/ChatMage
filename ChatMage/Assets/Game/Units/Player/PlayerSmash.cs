using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSmash : PlayerComponent
{
    public event SimpleEvent onSmashGained;
    public event SimpleEvent onSmashStarted;
    public event SimpleEvent onSmashCompleted;

    public bool SmashEquipped { get { return smash != null; } }
    public Smash Smash { get { return smash; } }
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
        if (!hasSmash || smash == null || controller.vehicle.IsDead)
            return;
        hasSmash = false;

        if (onSmashStarted != null)
            onSmashStarted();

        smash.OnSmash(
            delegate ()
            {
                if (onSmashCompleted != null)
                    onSmashCompleted.Invoke();
            });
    }
}
