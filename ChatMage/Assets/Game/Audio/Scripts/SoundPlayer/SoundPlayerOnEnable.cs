using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerOnEnable : SoundPlayer
{
    public float addDelay = 0;

    void OnEnable()
    {
        this.DelayedCall(PlaySound, addDelay);
    }
}
