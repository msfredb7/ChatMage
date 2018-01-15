using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerOnStart : SoundPlayer {

    public float addDelay = 0;

    new void Start()
    {
        base.Start();
        this.DelayedCall(PlaySound, addDelay);
    }
}
