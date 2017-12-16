using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITM_CarePackage : Item
{

    public CarePackage carePackagePrefab;

    public int killsRequired = 4;
    private int killstreak;
    private bool canCount = true;

    public override void OnGameReady()
    {
        killstreak = 0;
    }

    public override void OnGameStarted()
    {
        ResetCounter();
        canCount = true;
        player.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
    }

    public override void OnUpdate()
    {

    }

    private void PlayerStats_onUnitKilled(Unit unit)
    {
        if(canCount && !(unit is CarePackage))
        {
            killstreak++;
            if (killstreak >= killsRequired)
            {
                SendPackage();
                ResetCounter();
            }
        }
    }

    void ResetCounter()
    {
        killstreak = 0;
    }

    void SendPackage()
    {
        GameCamera cam = Game.instance.gameCamera;

        canCount = false;

        float x = (GameCamera.DEFAULT_SCREEN_WIDTH / 2) * 0.75f;
        float y = (GameCamera.DEFAULT_SCREEN_HEIGHT / 2) * 0.75f;
        Vector2 spawnDelta = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
        spawnDelta = cam.AdjustVector(spawnDelta);

        Vector2 spawnPos = cam.Center + spawnDelta;
        spawnPos = Game.instance.map.VerifyPosition(spawnPos, carePackagePrefab.unitWidth);

        Game.instance.SpawnUnit(carePackagePrefab, spawnPos)
            .OnDeath += ITM_CarePackage_onDeath;
    }

    //On �coute � la mort du carepackage. Comme �a, les killstreak ne continue pas tant qu'il n'a pas pris son package (sinon c op)
    private void ITM_CarePackage_onDeath(Unit unit)
    {
        canCount = true;
    }

    public override void Equip()
    {
        throw new System.NotImplementedException();
    }

    public override void Unequip()
    {
        throw new System.NotImplementedException();
    }
}
