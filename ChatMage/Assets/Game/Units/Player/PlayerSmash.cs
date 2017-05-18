using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSmash : PlayerComponent
{
    [Header("Debug")]
    public Smash defaultSmash;
    
    [System.NonSerialized]
    public UnityEvent onSmashAppear = new UnityEvent();
    [System.NonSerialized]
    public UnityEvent onSmashGained = new UnityEvent();
    [System.NonSerialized]
    public UnityEvent onSmashUsed = new UnityEvent();


    public bool HasSmash { get { return hasSmash; } }
    private bool hasSmash;

    [System.NonSerialized]
    private Smash smash;

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        //Temporaire
        SetSmash(defaultSmash);
    }

    public override void OnGameReady()
    {
        smash.OnGameReady();
    }

    public override void OnGameStarted()
    {
        smash.OnGameStarted();

        //Temporaire
        StartSmashCooldown();
    }

    public void SetSmash(Smash smash)
    {
        this.smash = smash;
        smash.Init(controller);
    }

    //Appartion de la boule !
    void OnSmashRefresh()
    {
        onSmashAppear.Invoke();

        //Temporaire
        GainSmash();
    }

    //Smash gained !
    public void GainSmash()
    {
        hasSmash = true;
        onSmashGained.Invoke();
    }

    //Utilisation du smash !
    public void SmashClick()
    {
        if (!hasSmash)
            return;
        hasSmash = false;

        smash.OnSmash();
        onSmashUsed.Invoke();

        StartSmashCooldown();
    }

    private void StartSmashCooldown()
    {
        //TODO Mettre un vrai system de cooldown ou countdown
        DelayManager.CallTo(OnSmashRefresh, 3);
    }
}
