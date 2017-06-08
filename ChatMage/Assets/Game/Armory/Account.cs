using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Account : BaseManager<Account>
{
    private const string COINS_KEY = "coins";

    // Compte externe comme google ou paiement etc.
    // public GoogleAccount account;

    // Coins
    private int coins = 0;
    public SimpleEvent onBalanceChange;

    public static int GetLastSavedMoney() { return PlayerPrefs.GetInt("Money"); }

    public override void Init()
    {
        Load();
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
    public bool AddMoney(int amount)
    {
        int moneyResult = coins + amount;
        if (moneyResult < 0)
            return false;

        coins = moneyResult;
        if (onBalanceChange != null)
            onBalanceChange();

        Save();

        return true;
    }
}
