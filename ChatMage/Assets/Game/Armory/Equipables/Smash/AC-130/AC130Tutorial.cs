using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AC130Tutorial : MonoBehaviour {

    public Text text;

	void Start ()
    {
        if (Application.isMobilePlatform)
        {
            text.text = "TAP YOUR SCREEN";
        } else
        {
            text.text = "CLICK YOUR SCREEN";
        }
    }
}
