using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_Spinner : Item {

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        Game.Instance.Player.vehicle.canSpin = true;
    }
    public override void Unequip()
    {
        base.Unequip();
        Game.Instance.Player.vehicle.canSpin = true;
    }
}
