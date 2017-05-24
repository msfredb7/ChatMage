using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour {

    void Start()
    {
        if (Account.instance == null)
            Scenes.Load("MainMenu");
    }

	public void BuyLootBox()
    {
        new LootBox(Account.instance.armory,LootBox.LootBoxType.common);
    }

    public void BuySlots()
    {
        Account.instance.armory.BuyItemSlots(1,-10);
    }

    public void GetMoney()
    {
        Account.instance.ChangeMoney(10);
    }

    public void LoadScene(string name)
    {
        LoadingScreen.TransitionTo(name, null);
    }
}
