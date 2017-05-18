using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    [System.NonSerialized]
    protected PlayerController controller;

    public virtual void Init(PlayerController controller)
    {
        this.controller = controller;
    }

    public abstract void OnGameReady();

    public abstract void OnGameStarted();
}
