using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerOnEnable : SoundPlayer
{
    public float addDelay = 0;

    void OnEnable()
    {
        DelayManager.LocalCallTo(PlaySound, addDelay, this);
    }
}
