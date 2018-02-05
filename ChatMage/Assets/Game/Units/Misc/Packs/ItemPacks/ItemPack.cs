using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPack : Pack
{
    [System.NonSerialized]
    public Item itemReference;

    public DataSaver firstPickupSave;

    private string firstPickUpKey = "FPUK";

    public void SetItem(Item itemReference)
    {
        this.itemReference = itemReference;
    }

    protected override void OnPickUp()
    {
        PlayerController player = Game.Instance != null ? Game.Instance.Player : null;

        if (itemReference != null && player != null)
        {
            // Equip item
            player.playerItems.Equip(itemReference);
            // First Time Its Equiped ?
            string key = firstPickUpKey + itemReference.name;
            // Save Exist doesn't exist so its the first time
            if (!firstPickupSave.ContainsBool(key))
            {
                // Create the save
                firstPickupSave.SetBool(key, true);
                firstPickupSave.SaveAsync();

                // Trigger UI Anim
                //Game.Instance.ui.
            } // else it isn't the first time so its ok
        }
    }
}
