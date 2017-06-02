using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Account : BaseManager<Account> {

    // Compte externe comme google ou paiement etc.
    // public GoogleAccount account;

    // Money
    private int money = 0;
    public UnityEvent onMoneyChanged = new UnityEvent();

    public static int GetLastSavedMoney() { return PlayerPrefs.GetInt("Money"); }

    public override void Init()
    {
        Load();
        CompleteInit();
    }

    private void OnDestroy()
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Load()
    {
        if(GameSaves.instance.ContainsInt(GameSaves.Type.Account, "money"))
            money = GameSaves.instance.GetInt(GameSaves.Type.Account, "money");
        else
            GameSaves.instance.SetInt(GameSaves.Type.Account, "money", money);
    }

    public void Save()
    {
        GameSaves.instance.SetInt(GameSaves.Type.Account,"money",money);
    }

    public int GetMoney()
    {
        return money;
    }

    /// <summary>
    /// Ajout ou retire un certain montant d'argent au compte du joueur
    /// </summary>
    /// <returns>Retourne si le changement a reussi ou pas</returns>
    public bool ChangeMoney(int amount)
    {
        int moneyResult = money + amount;
        if (moneyResult < 0)
            return false;

        money = moneyResult;
        onMoneyChanged.Invoke();

        return true;
    }
}
