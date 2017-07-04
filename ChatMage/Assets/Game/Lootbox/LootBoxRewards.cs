using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure de données contenant les infos qu'on envoit au UI
public class LootBoxRewards {

    public EquipablePreview equipable;
    public int cashAmount;

	public LootBoxRewards(EquipablePreview equipable, int cashAmount)
    {
        this.equipable = equipable;
        this.cashAmount = cashAmount;
    }
}
