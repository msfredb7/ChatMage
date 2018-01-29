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
    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "fade bot":
                Game.Instance.map.mapping.GetTaggedObject("bot").GetComponent<SpriteGroup>().DOFade(1, 0.5f).SetUpdate(true);
                break;
        }
    }
}
