using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplayScript : MonoBehaviour {

    public Text text;

    private void Update()
    {
        if(Account.instance != null)
            text.text = Account.instance.GetMoney().ToString();
    }
}
