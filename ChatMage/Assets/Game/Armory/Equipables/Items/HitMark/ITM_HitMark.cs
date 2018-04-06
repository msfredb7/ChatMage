using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_HitMark : Item {

    public GameObject hitMarkPrefab;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);
        Game.Instance.Player.playerCarTriggers.onUnitKilled += PlayerCarTriggers_onUnitKilled;
    }

    private void PlayerCarTriggers_onUnitKilled(Unit unit, CarSide carTrigger, ColliderInfo other, ColliderListener listener)
    {
        Instantiate(hitMarkPrefab, unit.transform);
        Game.Instance.commonSfx.HitMark();
    }

    public override void Unequip()
    {
        base.Unequip();
    }
}
