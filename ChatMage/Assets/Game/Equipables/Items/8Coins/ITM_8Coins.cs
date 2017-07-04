using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_8Coins : Item {

    public GameObject coinPrefab;
    public GameObject rewardPrefab;

    public int amount = 8;

    public float cooldown = 5;

    private int coinCounter;
    private bool cooldownStarted;
    private float cooldownCounter;

    public override void OnGameReady()
    {
        coinCounter = 0;
        cooldownStarted = false;
    }

    public override void OnGameStarted()
    {
        SpawnCoin().GetComponent<CoinToCatch>().coinCatch += CoinCatch;
    }

    public override void OnUpdate()
    {
        if(cooldownCounter < 0 && cooldownStarted)
        {
            coinCounter = 0;
            SpawnCoin().GetComponent<CoinToCatch>().coinCatch += CoinCatch;
            cooldownStarted = false;
        }
        cooldownCounter -= player.vehicle.DeltaTime();
    }

    GameObject SpawnCoin()
    {
        return SpawnObjectRandomLocation(coinPrefab);
    }

    GameObject SpawnObjectRandomLocation(GameObject prefab)
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

        return Instantiate(prefab, pos + (Vector2)Game.instance.gameCamera.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    void CoinCatch()
    {
        Debug.Log("Coin Catch !");
        if (coinCounter >= amount)
        {
            SpawnObjectRandomLocation(rewardPrefab);
            cooldownCounter = cooldown;
            cooldownStarted = true;
        } else
        {
            coinCounter++;
            SpawnCoin().GetComponent<CoinToCatch>().coinCatch += CoinCatch;
        }
    }
}
