using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Manager;

public class MoneyDisplayScript : MonoBehaviour
{

    public Text text;

    void Start()
    {
        MasterManager.Sync(OnSync);
    }

    void OnSync()
    {
        UpdateDisplayChange();
        Account.instance.onBalanceChange += UpdateDisplayChange;
    }

    void UpdateDisplayChange()
    {
        text.text = Account.instance.Coins.ToString();
    }

    void OnDestroy()
    {
        //Remove listener
        Account.instance.onBalanceChange -= UpdateDisplayChange;
    }
}
