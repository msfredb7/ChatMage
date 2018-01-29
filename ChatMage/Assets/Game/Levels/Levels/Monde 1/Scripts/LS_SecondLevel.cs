using CCC._2D;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_SecondLevel : LevelScript
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
