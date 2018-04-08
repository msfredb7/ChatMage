using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCheering : MonoBehaviour {

    public float cheerCooldown = 5f;

    private float timer;

    private bool canDoSound;

    void Start()
    {
        timer = cheerCooldown;
        canDoSound = true;

        Game.Instance.onGameReady += Instance_onGameReady;
    }

    private void Instance_onGameReady()
    {
        if (Game.Instance != null)
            Game.Instance.Player.playerStats.OnUnitKilled += PlayerStats_OnUnitKilled;
    }

    private void PlayerStats_OnUnitKilled(Unit unit)
    {
        if (unit is JesusV2Vehicle)
            return;
        if (canDoSound)
            DoCrowdSoundEffect();
    }

    void Update ()
    {
        if (!canDoSound)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                canDoSound = true;
        }
    }

    void DoCrowdSoundEffect()
    {
        if (Game.Instance != null)
        {
            Game.Instance.commonSfx.CrowdScreaming();
            canDoSound = false;
            timer = cheerCooldown;
        }
    }
}
