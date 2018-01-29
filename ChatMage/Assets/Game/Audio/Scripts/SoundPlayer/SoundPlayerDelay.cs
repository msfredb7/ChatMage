﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerDelay : SoundPlayer
{
    public float addDelay = 0;

    public void Play()
    {
        this.DelayedCall(PlaySound, addDelay);
    }
}
