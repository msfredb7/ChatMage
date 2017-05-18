using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthScript : MonoBehaviour {

    public Sprite emptyHearth;
    public Sprite hearth;

    public void On()
    {
        GetComponent<Image>().sprite = hearth;
    }

    public void Off()
    {
        GetComponent<Image>().sprite = emptyHearth;
    }

    public void Armor()
    {
        
    }
}
