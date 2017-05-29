using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutButton : MonoBehaviour {

    public Text uiText;

    public void ChangeLoadoutButton(string text)
    {
        uiText.text = text;
    }
}
