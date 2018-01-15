using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Objet utile pour généré l'effet d'un lootbox, ceci ne gère pas les animations
public class LootBox
{
    public List<LootBoxRewards> rewards;
    public LootBoxRef.PickReport.Message specialMessage;

    public static void NewLootbox(string lootboxRef, Action<LootBox> onComplete, bool goldified = false)
    {
        ResourceLoader.LoadLootBoxRefAsync(lootboxRef, delegate (LootBoxRef reference)
        {
            LootBox lootbox = null;
            lootbox = new LootBox(reference, delegate ()
            {
                if (lootbox == null)
                    throw new Exception("da fuck is dat shit. ya qqchose qui a pas loadé, ya boi.");

                onComplete(lootbox);
            },
            goldified);
        });
    }

    private LootBox(LootBoxRef lootbox, Action onComplete, bool goldified = false)
    {
        rewards = new List<LootBoxRewards>();

        // Load les Equipables dans la Lootbox Ref
        lootbox.PickRewards(goldified, delegate (LootBoxRef.PickReport report)
        {
            specialMessage = report.specialMessage;
            List<EquipablePreview> equipableRewards = report.pickedEquipables;

            bool shouldSaveArmory = false;
            bool shouldSaveAccount = false;
            for (int i = 0; i < equipableRewards.Count; i++)
            {
                equipableRewards[i].Load();

                if (equipableRewards[i].Unlocked) // Le joueur a deja l'item
                {
                    rewards.Add(new LootBoxRewards(equipableRewards[i], StorePrice.duplicateReward));

                    // Et on ajoute la recompense dans le compte du joueur
                    Account.instance.Command(StorePrice.CommandType.duplicateReward, saveAfterwards: false);
                    shouldSaveAccount = true;
                }
                else
                {
                    rewards.Add(new LootBoxRewards(equipableRewards[i], -1));
                    equipableRewards[i].MarkAsUnlocked();
                    shouldSaveArmory = true;
                }
            }

            if (shouldSaveArmory)
                GameSaves.instance.SaveDataAsync(GameSaves.Type.Armory, null);
            if (shouldSaveAccount)
                GameSaves.instance.SaveDataAsync(GameSaves.Type.Account, null);


            string txtMessage = LootBoxRef.PickReport.MessageToString(specialMessage);

            if (txtMessage != "")
                Debug.LogWarning(txtMessage);


            PersistentLoader.instance.CallNextFrame(onComplete);
        });
    }
}
