using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class demoLevelBehavior : LevelBehavior {

    public override void OnBegin(LevelScript levelScript)
    {
        this.levelScript = levelScript;
        onEnding = new UnityEvent();
        DelayManager.CallTo(delegate ()
        {
            OnComplete();
        }, 5);
    }
    public override void OnUpdate()
    {

    }
    public override void OnComplete()
    {
        onEnding.Invoke();
    }
}
