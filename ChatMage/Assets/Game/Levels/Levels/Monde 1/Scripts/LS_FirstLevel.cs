using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;
using CCC._2D;
using DG.Tweening;

public class LS_FirstLevel : LevelScript
{
    protected override void OnGameReady()
    {
        Game.instance.smashManager.smashEnabled = false;
        Game.instance.ui.smashDisplay.canBeShown = false;
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "fade bot":
                Game.instance.map.mapping.GetTaggedObject("bot").GetComponent<SpriteGroup>().DOFade(1, 0.5f).SetUpdate(true);
                break;
        }
    }
}
