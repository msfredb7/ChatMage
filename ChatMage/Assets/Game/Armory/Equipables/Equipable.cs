using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public abstract class Equipable : BaseScriptableObject
{
    [fsIgnore]
    public PlayerController player;

    public virtual void Init(PlayerController player)
    {
        this.player = player;
        Game.instance.onDestroy += ClearReferences;
    }

    public abstract void OnGameReady();
    public abstract void OnGameStarted();
    public abstract void OnUpdate();

    protected virtual void ClearReferences()
    {
        if (Game.instance != null)
            Game.instance.onDestroy -= ClearReferences;
        player = null;
    }
}
