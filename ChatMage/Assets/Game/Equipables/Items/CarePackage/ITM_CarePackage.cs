using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_CarePackage : Item
{

    public GameObject carePackagePrefab;

    public int killsRequired = 4;
    private int killstreak;

    public override void OnGameReady()
    {
        killstreak = 0;
    }

    public override void OnGameStarted()
    {
        ResetCounter();
        player.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
    }

    public override void OnUpdate()
    {

    }

    private void PlayerStats_onUnitKilled(Unit unit)
    {
        killstreak++;
        Debug.Log("Killstreak: " + killstreak);

        if (killstreak >= killsRequired)
        {
            SendPackage();
            ResetCounter();
        }
    }

    void ResetCounter()
    {
        killstreak = 0;
    }

    void SendPackage()
    {
        GameCamera cam = Game.instance.gameCamera;

        //Get Random pos around screen
        Vector2 pos = Vector2.zero;

        //Accot� sur le planfond/plancher OU le cot� droit/gauche ?
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            //    Donne:        soit -1 ou 1            , random entre -1f et 1f
            pos = new Vector2(UnityEngine.Random.Range(0, 2) * 2 - 1, UnityEngine.Random.Range(-1f, 1f));
        }
        else
        {
            //    Donne:     random entre -1f et 1f,        soit -1 ou 1 
            pos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0, 2) * 2 - 1);
        }


        //Scale la position au bordure de l'�cran
        pos.Scale(cam.ScreenSize * 0.45f);
        
        Vector2 spawnPos = pos + cam.Center;
        Instantiate(carePackagePrefab, spawnPos, Quaternion.identity);
    }
}
