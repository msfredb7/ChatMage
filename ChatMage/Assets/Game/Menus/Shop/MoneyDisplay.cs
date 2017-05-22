using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour {

	void Update ()
    {
        if(Account.instance != null)
            GetComponent<Text>().text = "Money : " + Account.instance.GetMoney();
	}
}
