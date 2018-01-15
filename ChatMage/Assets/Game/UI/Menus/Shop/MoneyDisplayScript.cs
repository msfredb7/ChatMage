using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoneyDisplayScript : MonoBehaviour
{

    public Text text;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(OnSync);
    }

    void OnSync()
    {
        UpdateDisplayChange();
        Account.instance.onCoinsChange += UpdateDisplayChange;
    }

    void UpdateDisplayChange()
    {
        text.text = Account.instance.Coins.ToString();
    }

    void OnDestroy()
    {
        //Remove listener
        Account.instance.onCoinsChange -= UpdateDisplayChange;
    }
}
