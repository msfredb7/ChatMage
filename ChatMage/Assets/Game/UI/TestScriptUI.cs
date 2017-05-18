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
            //Game.instance.Player.GetComponent<PlayerStats>().Init();
            display.Init();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Game.instance.Player.GetComponent<PlayerStats>().Hit();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Game.instance.Player.GetComponent<PlayerStats>().health.MAX++;
            Debug.Log(Game.instance.Player.GetComponent<PlayerStats>().health.MAX);
            display.ChangeMaxHP();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Game.instance.Player.GetComponent<PlayerStats>().health.MAX--;
            Debug.Log(Game.instance.Player.GetComponent<PlayerStats>().health.MAX);
            display.ChangeMaxHP();
        }
    }
}
