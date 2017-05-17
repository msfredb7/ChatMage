using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LevelBehavior {
    protected LevelScript levelScript;
    public UnityEvent onEnding;
    public abstract void OnBegin(LevelScript levelScript);
    public abstract void OnUpdate();
    public abstract void OnComplete();
}
