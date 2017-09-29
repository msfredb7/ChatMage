using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.Utility;

public class BrickWallDestruction : InGameAnimator
{
    Action onComplete;
    int animateHash = Animator.StringToHash("animate");

    public void Animate(Action onComplete)
    {
        this.onComplete = onComplete;
        GetComponent<Animator>().SetTrigger(animateHash);

        AddTimescaleListener();
    }
    private void OnComplete()
    {
        if (onComplete != null)
            onComplete();
        onComplete = null;

        RemoveTimescaleListener();
    }
}
