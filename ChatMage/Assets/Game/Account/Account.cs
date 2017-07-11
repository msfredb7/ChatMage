using CCC.Manager;
using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Account : BaseManager<Account>
{
    private const string COINS_KEY = "coins";

    // Coins
    private int coins = 0;
    public event SimpleEvent onCoinsChange;

    private Purchaser purchaser;

    public override void Init()
    {
        Load();
        purchaser = new Purchaser();
        purchaser.Init();
        CompleteInit();
    }

    public void Load()
    {
        if (GameSaves.instance.ContainsInt(GameSaves.Type.Account, COINS_KEY))
            coins = GameSaves.instance.GetInt(GameSaves.Type.Account, COINS_KEY);
        else
        {
            coins = 0;
            GameSaves.instance.SetInt(GameSaves.Type.Account, COINS_KEY, 0);
        }
    }

    public void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Account, COINS_KEY, coins);
        GameSaves.instance.SaveData(GameSaves.Type.Account);
    }

    public int Coins
    {
        get
        {
            return coins;
        }
    }

    /// <summary>
    /// Ajout ou retire un certain montant d'argent au compte du joueur
    /// </summary>
    /// <returns>Retourne si le changement a reussi ou pas</returns>
    public bool AddCoins(int amount, bool saveAfterwards = true)
    {
        int moneyResult = coins + amount;
        if (moneyResult < 0)
            return false;

        coins = moneyResult;
        if (onCoinsChange != null)
            onCoinsChange();

        if (saveAfterwards)
            Save();

        return true;
    }

    public bool Command(StorePrice.CommandType commandType, int amount = 1, bool saveAfterwards = true)
    {
        return AddCoins(StorePrice.GetPrice(commandType) * amount, saveAfterwards);
    }

    public void BuyCoins(int amount)
    {
        purchaser.BuyConsumable(amount);
    }

    public void BuyFullGame()
    {
        purchaser.BuyTheGame();
    }
}
