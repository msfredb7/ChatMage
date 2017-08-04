using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    public string lootboxRefName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LootBox.NewLootbox(lootboxRefName, delegate (LootBox lootbox)
            {
                for (int i = 0; i < lootbox.rewards.Count; i++)
                {
                    string txt = "Reward: " + lootbox.rewards[i].equipable.displayName;
                    if (lootbox.rewards[i].goldAmount > 0)
                        txt += "  (Duplicate: +" + lootbox.rewards[i].goldAmount + "g)";
                    print(txt);
                }
            });
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LootBox.NewLootbox(lootboxRefName, delegate (LootBox lootbox)
            {
                for (int i = 0; i < lootbox.rewards.Count; i++)
                {
                    string txt = "Reward: " + lootbox.rewards[i].equipable.displayName;
                    if (lootbox.rewards[i].goldAmount > 0)
                        txt += "  (Duplicate: +" + lootbox.rewards[i].goldAmount + "g)";
                    print(txt);
                }
            }, true);
        }
    }
}