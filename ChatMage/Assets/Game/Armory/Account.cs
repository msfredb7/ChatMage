using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Account : BaseManager<Account> {

    // Money
    private int money;
    public UnityEvent onMoneyChanged = new UnityEvent();

    // Armory
    public Armory armory;

    public static int GetLastSavedMoney() { return PlayerPrefs.GetInt("Money"); }

    public override void Init()
    {
        Load();
        armory = GetComponent<Armory>();
    }

    public void Load()
    {
        money = PlayerPrefs.GetInt("Money");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
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
        else money = moneyResult;
        onMoneyChanged.Invoke();
        return true;
    }
}
