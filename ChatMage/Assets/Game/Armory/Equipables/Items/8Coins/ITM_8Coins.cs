using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Math;
using DG.Tweening;
using FullInspector;

public class ITM_8Coins : Item
{
    private const int PATTERN_COUNT = 2;

    [InspectorHeader("Linking")]
    public RedCoin coinPrefab;
    public Unit rewardPrefab;

    [InspectorMargin(12), InspectorHeader("Settings")]
    public int coinsCount = 4;
    public float cooldown = 5;

    [InspectorMargin(12), InspectorHeader("Flash")]
    public float flashBefore = 2;
    public float flashInterval = 0.33f;

    [System.NonSerialized, fsIgnore]
    private RedCoin[] coins;
    [System.NonSerialized, fsIgnore]
    private int remainingCoins;
    [System.NonSerialized, fsIgnore]
    private float spawnTimer;
    [System.NonSerialized, fsIgnore]
    private float waveRemainingDuration = -1;
    [System.NonSerialized, fsIgnore]
    private Sequence flashSeqence;
    [System.NonSerialized, fsIgnore]
    private int patternIndex = 0;


    public override void Init(PlayerController player)
    {
        base.Init(player);
        SpawnCoins();
    }

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        patternIndex = Random.Range(0, PATTERN_COUNT);
        spawnTimer = cooldown;
    }

    private void StartCoinsWave()
    {
        float waveDuration = 0;
        switch (patternIndex)
        {
            default:
            case 0:
                waveDuration = Pattern0();
                break;
            case 1:
                waveDuration = GetPattern1();
                break;
        }

        waveRemainingDuration = waveDuration;

        for (int i = 0; i < coins.Length; i++)
            coins[i].gameObject.SetActive(true);

        remainingCoins = coins.Length;

        patternIndex++;
        patternIndex = patternIndex.Mod(PATTERN_COUNT);
    }

    private void SpawnCoins()
    {
        coins = new RedCoin[coinsCount];
        for (int i = 0; i < coinsCount; i++)
        {
            RedCoin newCoin = Instantiate(coinPrefab, Game.instance.unitsContainer);
            newCoin.onDeath = OnCoinTaken;
            coins[i] = newCoin;
        }
    }

    private void OnCoinTaken(RedCoin coin)
    {
        remainingCoins--;
        if (remainingCoins == 0)
            Win(coin.transform.position);

    }

    private void Win(Vector2 position)
    {
        //Spawn reward
        Game.instance.SpawnUnit(rewardPrefab, position);
        OnWaveFinished();
    }

    private void Lose()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].gameObject.SetActive(false);
        }
        remainingCoins = 0;
        OnWaveFinished();
    }

    private void OnWaveFinished()
    {
        waveRemainingDuration = -1;
        spawnTimer = cooldown * player.playerStats.cooldownMultiplier;
    }

    public override void OnUpdate()
    {
        if (waveRemainingDuration < 0)
        {
            spawnTimer -= player.vehicle.DeltaTime();

            if (spawnTimer <= 0)
                StartCoinsWave();
        }
        else
        {
            float wasRemains = waveRemainingDuration;
            waveRemainingDuration -= player.vehicle.DeltaTime();
            if (waveRemainingDuration <= 0)
                Lose();

            if (wasRemains > flashBefore && waveRemainingDuration < flashBefore)
                FlashCoins(flashBefore);
        }
    }

    private void FlashCoins(float duration)
    {
        flashSeqence = DOTween.Sequence();

        flashSeqence.InsertCallback(flashInterval, delegate ()
        {
            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].renderer.enabled = false;
            }
        });
        flashSeqence.InsertCallback(flashInterval * 2, delegate ()
        {
            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].renderer.enabled = true;
            }
        }).SetLoops(Mathf.CeilToInt(duration / (flashInterval * 2)), LoopType.Restart);
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        remainingCoins = 0;
        coins = null;
        waveRemainingDuration = -1;
        flashSeqence.Kill();
        flashSeqence = null;
    }



    //--------------------------POSITION PATTERNS--------------------------//

    //Les 4 coins de la map
    private float Pattern0()
    {
        GameCamera cam = Game.instance.gameCamera;
        Vector2 halfedScreenSize = cam.ScreenSize / 2;
        Vector2 v = Vector2.zero;

        for (int i = 0; i < coins.Length; i++)
        {
            Vector2 randomV = Vectors.RandomVector2(0, 360, 0, 1f);

            int s = i.Mod(4);
            float profondeur = 0.6f;

            switch (s)
            {
                case 0:
                    v = halfedScreenSize.FlippedX() * profondeur;
                    break;
                case 1:
                    v = halfedScreenSize * profondeur;
                    break;
                case 2:
                    v = -halfedScreenSize * profondeur;
                    break;
                case 3:
                    v = halfedScreenSize.FlippedY() * profondeur;
                    break;
            }

            coins[i].transform.position = v + cam.Center + randomV;
        }
        return 10;
    }

    //Une ligne de coins
    private float GetPattern1()
    {
        float angle = Random.value * 360;
        float startDistance = Random.value * 2 + 2.25f;

        Vector2 start = angle.ToVector() * startDistance;

        angle = Random.value * 90 + angle + 135;
        Vector2 dir = (angle).ToVector();

        Vector2 camCenter = Game.instance.gameCamera.Center;

        float espacement = 0.85f;

        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.position = camCenter + start + dir * (i * espacement);
        }
        return 7;
    }

    public override void Equip(int duplicateIndex)
    {
        throw new System.NotImplementedException();
    }

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}
