using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_CarePackage : Item {

    public GameObject carePackagePrefab;

    public int unitAmount = 4;
    private int unitCounter;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        ResetCounter();
        player.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
    }

    private void PlayerStats_onUnitKilled(Unit unit)
    {
        unitCounter++;
    }

    public override void OnUpdate()
    {
        if(unitCounter >= unitAmount)
        {
            SendPackage();
            ResetCounter();
        }
    }

    void ResetCounter()
    {
        unitCounter = 0;
    }

    void SendPackage()
    {
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
        pos.Scale(Game.instance.gameCamera.ScreenSize * 0.45f);

        SpawnUnitRelativeToTransform(pos, Game.instance.gameCamera.transform);
    }

    private void SpawnUnitRelativeToTransform(Vector2 relativePosition, Transform transform)
    {
        Instantiate(carePackagePrefab, relativePosition + (Vector2)transform.position,Quaternion.Euler(new Vector3(0,0,0)));
    }
}
