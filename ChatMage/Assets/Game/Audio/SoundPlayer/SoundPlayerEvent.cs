using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundPlayerEvent : SoundPlayer
{
    public UnityEvent onBegin = new UnityEvent();
    public float addDelay = 0;

    new void Start()
    {
        base.Start();
        onBegin.AddListener(delegate ()
        {
            DelayManager.LocalCallTo(PlaySound, addDelay, this);
        });
    }
}
