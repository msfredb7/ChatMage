
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSmash : PlayerComponent
{
    public event SimpleEvent onSmashStarted;
    public event SimpleEvent onSmashCompleted;

    public bool SmashEquipped { get { return smash != null; } }
    public Smash Smash { get { return smash; } }
    public bool SmashInProgress { get; private set; }

    [System.NonSerialized]
    private Smash smash;
    [System.NonSerialized]
    private SmashManager smashManager;

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
        smashManager = Game.Instance.smashManager;
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
        SmashInProgress = false;
    }

    public override void OnGameStarted()
    {
        if (smash != null)
            smash.OnGameStarted();
    }

    public void Update()
    {
        if (SmashInProgress)
            smash.OnUpdate();
    }

    //Utilisation du smash !
    public void SmashClick()
    {
        if (SmashInProgress || !smashManager.CanSmash())
            return;

        OnStartSmash();
        smash.OnSmash(OnEndSmash);
    }

    void OnStartSmash()
    {
        SmashInProgress = true;

        if (!smash.canGainJuiceWhileSmashing)
            smashManager.canGainJuice.Lock(SM_LOCK_KEY);

        if (onSmashStarted != null)
            onSmashStarted();
    }

    private const string SM_LOCK_KEY = "inSm";

    void OnEndSmash()
    {
        SmashInProgress = false;
        smashManager.canGainJuice.UnlockAll(SM_LOCK_KEY);

        if (smash.clearAllJuiceOnCompletion)
            Game.Instance.smashManager.RemoveAllJuice();

        if (onSmashCompleted != null)
            onSmashCompleted.Invoke();
    }
}
