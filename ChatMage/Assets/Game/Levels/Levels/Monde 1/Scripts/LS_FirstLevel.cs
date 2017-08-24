using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;

public class LS_FirstLevel : LevelScript
{
    protected override void OnGameReady()
    {
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;
    }
}
