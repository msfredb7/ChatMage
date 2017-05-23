using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitQueue
{
    Action onComplete;
    int count = 0;
    public bool canTriggerAction = true;
    public bool IsDone
    {
        get { return isDone; }
    }
    bool isDone = false;

    public InitQueue(Action onComplete)
    {
        this.onComplete = onComplete;
    }
    public Action Register()
    {
        count++;
        return OnCompleteAnyInit;
    }
    void OnCompleteAnyInit()
    {
        count--;
        if (count <= 0 && !isDone)
        {
            isDone = true;
            if (canTriggerAction)
                onComplete();
        }
    }
}
