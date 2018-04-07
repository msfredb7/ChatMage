using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPack : Pack
{
    private ItemSpawnerSettings settings;
    [Header("Specific Item Reference")]
    public Item item;
    [Header("Saver")]
    public DataSaver firstPickupSave;

    private bool shouldSpawnSpecial;

    private string firstPickUpKey = "FPUK";

    public void SetInfo(ItemSpawnerSettings settings, bool specialItem)
    {
        this.settings = settings;
        shouldSpawnSpecial = specialItem;
    }

    protected override void OnPickUp()
    {
        PlayerController player = Game.Instance != null ? Game.Instance.Player : null;

        if (player != null)
        {
            if(item == null)
            {
                if(settings != null)
                {
                    // Get Item
                    if (shouldSpawnSpecial)
                    {
                        item = settings.GainSpecialItem();
                        if (item == null)
                            item = settings.GainItem();
                    }
                    else
                        item = settings.GainItem();
                }
                else
                {
                    Debug.LogError("Error, no settings in item packs");
                }
            }

            // Equip item
            player.playerItems.Equip(item);
            // First Time Its Equiped ?
            string key = firstPickUpKey + item.name;
            // Save Exist doesn't exist so its the first time
            if (!firstPickupSave.ContainsBool(key))
            {
                // Create the save
                firstPickupSave.SetBool(key, true);
                firstPickupSave.SaveAsync();

                // Trigger UI Anim
                Game.Instance.ui.itemNotification.Notify(item);
            } // else it isn't the first time so its ok
        }
    }
}
