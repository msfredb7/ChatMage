using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Spinner : Item
{
    public AudioClip spinnerSFX;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        Game.Instance.Player.vehicle.CanSpin = true;
    }
    public override void Unequip()
    {
        base.Unequip();

        // Are we the last spinner ?
        if (player.playerItems.GetDuplicateCount(originalAssetID) == 0)
            Game.Instance.Player.vehicle.CanSpin = false;
    }

    public int SimilarItemCount
    {
        get
        {
            int count = 0;
            var list = player.playerItems.items;
            foreach (var item in list)
            {
                if (item is ITM_Spinner)
                    count++;
            }
            return count;
        }
    }

    public override float GetWeight()
    {
        return SimilarItemCount >= 1 ? 0 : base.GetWeight();
    }
}
