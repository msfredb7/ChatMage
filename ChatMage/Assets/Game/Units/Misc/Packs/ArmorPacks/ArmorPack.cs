using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmorPack : Pack
{
    [System.NonSerialized]
    public int armorValue = 1;

    protected override void OnPickUp()
    {
        Game.Instance.Player.playerStats.GiveArmor(armorValue);
    }
}
