using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCheering : MonoBehaviour {

    public float cheerCooldown = 5f;
    public AudioPlayable crowdCheer;

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
        if (!Game.Instance.map.allowCrowdCheering)
            return;
        if (unit is IAttackable && ((IAttackable)unit).GetSmashJuiceReward() <= 0)
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
            DefaultAudioSources.PlayStaticSFX(crowdCheer);
            canDoSound = false;
            timer = cheerCooldown;
        }
    }
}
