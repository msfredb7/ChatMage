﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashDisplay : MonoBehaviour
{
    public Text text;

    SmashManager smasher;

    public void Init()
    {
        smasher = Game.instance.smashManager;
    }

    void Update()
    {
        if (smasher != null)
            SetDisplay(smasher.RemainingTime);
    }

    void SetDisplay(float newValue)
    {
        text.text = "Smash in: " + (int)newValue;
    }
}
