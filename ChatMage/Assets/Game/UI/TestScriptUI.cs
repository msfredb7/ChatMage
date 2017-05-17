using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptUI : MonoBehaviour {

    public HealthDisplay display;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            display.GiveHP(1);
            Game.instance.Player.GetComponent<PlayerStats>().health++;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            display.Init();
        }
    }
}
