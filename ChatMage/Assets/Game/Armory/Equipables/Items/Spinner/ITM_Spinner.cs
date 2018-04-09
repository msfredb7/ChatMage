using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Spinner : Item
{
    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        Game.Instance.Player.vehicle.CanSpin = true;
    }
    public override void Unequip()
    {
        base.Unequip();

        // Are we the last spinner ?
        if (player.playerItems.GetCountOf(originalAssetID) == 0)
            Game.Instance.Player.vehicle.CanSpin = false;
    }

    public int SimilarItemCount
    {
        get
        {
            return Game.Instance.Player.playerItems.GetCountOf<ITM_Spinner>();
        }
    }

    public override float GetWeight()
    {
        return SimilarItemCount >= 1 ? 0 : base.GetWeight();
    }
}
