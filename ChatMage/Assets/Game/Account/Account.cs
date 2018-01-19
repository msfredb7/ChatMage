using CompleteProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

public class Account : MonoPersistent
{
    private const string COINS_KEY = "coins";

    // Coins
    private int coins = 0;
    public event SimpleEvent onCoinsChange;

    private Purchaser purchaser;
    public static Account instance;

    void Awake()
    {
        instance = this;
    }


    public override void Init(Action onComplete)
    {
        Load();
#if UNITY_ANDROID
        purchaser = new Purchaser();
        purchaser.Init();
#endif
        onComplete();
    }

    public void Load()
    {
        if (DataSaver.instance.ContainsInt(DataSaver.Type.Account, COINS_KEY))
            coins = DataSaver.instance.GetInt(DataSaver.Type.Account, COINS_KEY);
        else
        {
            coins = 0;
            DataSaver.instance.SetInt(DataSaver.Type.Account, COINS_KEY, 0);
        }
    }

    public void Save()
    {
        DataSaver.instance.SetInt(DataSaver.Type.Account, COINS_KEY, coins);
        DataSaver.instance.SaveData(DataSaver.Type.Account);
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
#if UNITY_ANDROID
    public void BuyCoins(int amount)
    {
        purchaser.BuyConsumable(amount);
    }

    public void BuyFullGame()
    {
        purchaser.BuyTheGame();
    }
#endif
}
