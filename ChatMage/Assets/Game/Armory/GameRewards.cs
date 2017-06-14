using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class GameRewards {

    public bool giveMoney;
    [InspectorShowIf("giveMoney")]
    public int amount;
    public bool giveLootBox;
    [InspectorShowIf("giveLootBox")]
    public List<LootBoxRef> lootboxs = new List<LootBoxRef>();

    // To open the lootbox : new LootBox(lootboxs[0].identifiant, delegate (List<EquipablePreview> rewards) { ... });
    // To give the money : Account.instance.Command(StorePrice.CommandType.customGoldAmount, amount);

    // Ajout de reward de slot ?
    // Ajout de reward d'objet ?
}
