using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Spinner : Item
{
    public AudioPlayable spinnerSFX;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        Game.Instance.Player.vehicle.CanSpin = true;
    }
    public override void Unequip()
    {
        base.Unequip();

        // Were we the last spinner ?
        if (player.playerItems.GetCountOf(originalAssetID) == 0)
            Game.Instance.Player.vehicle.CanSpin = false;
    }

    public override float GetWeight()
    {
        var playerItems = Game.Instance.Player.playerItems;

        // Si nous avons plus d'1 ChainChomp, on ne peut pas avoir de spinner
        // +
        // Si nous avons déjà un spinner, on en veut pas un 2e
        if (playerItems.GetCountOf<ITM_ChainChomp>() > 1 || playerItems.GetCountOf<ITM_Spinner>() > 0)
            return 0;

        return 1;
    }
}
