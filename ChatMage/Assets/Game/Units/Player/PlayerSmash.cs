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
    //public bool HasSmash { get { return hasSmash; } }
    //private bool hasSmash;
    private bool smashInProgress;

    [System.NonSerialized]
    private Smash smash;
    [System.NonSerialized]
    private SmashManager smashManager;

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
        smashManager = Game.instance.smashManager;
    }

    public void SetSmash(Smash smash)
    {
        this.smash = smash;
        if (smash != null)
            smash.Init(controller);

        smashManager.MaxJuice = smash.GetMaxJuice();
        smashManager.MinimumActivatableJuice = smash.GetMinJuice();
    }

    public override void OnGameReady()
    {

        if (smash != null)
            smash.OnGameReady();
        smashInProgress = false;
    }

    public override void OnGameStarted()
    {
        if (smash != null)
            smash.OnGameStarted();
    }

    private void Update()
    {
        if (smashInProgress)
            smash.OnUpdate();
    }

    //Smash gained !
    //public void GainSmash()
    //{
    //    hasSmash = true;
    //    if (onSmashGained != null)
    //        onSmashGained();
    //}

    //Utilisation du smash !
    public void SmashClick()
    {
        if (smashInProgress || !smashManager.CanSmash())
            return;

        OnStartSmash();
        smash.OnSmash(OnEndSmash);

        //if (!hasSmash || smash == null || controller.vehicle.IsDead)
        //    return;
        //hasSmash = false;

        //if (onSmashStarted != null)
        //    onSmashStarted();

        //smash.OnSmash(
        //    delegate ()
        //    {
        //        if (onSmashCompleted != null)
        //            onSmashCompleted.Invoke();
        //    });
    }

    void OnStartSmash()
    {
        smashInProgress = true;

        if (!smash.canGainJuiceWhileSmashing)
            smashManager.canGainJuice.Lock(SM_LOCK_KEY);

        if (onSmashStarted != null)
            onSmashStarted();
    }

    private const string SM_LOCK_KEY = "inSm";

    void OnEndSmash()
    {
        smashInProgress = false;
        smashManager.canGainJuice.UnlockAll(SM_LOCK_KEY);

        if (smash.clearAllJuiceOnCompletion)
            Game.instance.smashManager.RemoveAllJuice();

        if (onSmashCompleted != null)
            onSmashCompleted.Invoke();
    }
}
