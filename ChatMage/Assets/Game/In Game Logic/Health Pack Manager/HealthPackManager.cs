using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackManager : MonoBehaviour
{
    [Header("Linking")]
    public HealthPacks healthPackRefab;

    [Header("Debug Setting")]
    public bool debugPrints = false;

    [Header("Luck Settings (in %)")]
    public float lerpUpdateRate = 0.001f;
    public float lerpPerKill = 0.1f;
    public float lerpCeiling = 50;
    public float luckIncreaseByHealthDeficit = 4f;
    public float luckMultiplier = 1;

    public bool enableHealthPackSpawn = true;

    [ReadOnly, Header("Live Data")]
    public float spawnChance = 0;

    PlayerController player;

    public void Init(PlayerController controller)
    {
        controller.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
        player = controller;

    }

    void Update()
    {
        //On n'augmente pas si le joueur est full hp
        if (!Game.instance.gameStarted || player == null || player.playerStats.health >= player.playerStats.health.MAX)
            return;


        //On pourrait utiliser Game.instance.globalTimeScale, 
        // mais je ne pense pas que c'est nécessaire parce que c'est caché du joueur

        spawnChance = Mathf.Lerp(spawnChance, lerpCeiling, FixedLerp.Fix(lerpUpdateRate));
    }

    private void PlayerStats_onUnitKilled(Unit unit)
    {
        //On n'augmente pas si le joueur est full hp
        if (player.playerStats.health >= player.playerStats.health.MAX)
            return;

        //Increase
        spawnChance = Mathf.Lerp(spawnChance, lerpCeiling, lerpPerKill);

        //Try to spawn health pack
        TryToSpawnHealthPack(unit.Position);
    }

    public HealthPacks TryToSpawnHealthPack(Vector2 position)
    {
        if (player == null)
            return null;

        if (!enableHealthPackSpawn)
            return null;

        float realSpawnChance = spawnChance
            + (player.playerStats.health.MAX - player.playerStats.health) * luckIncreaseByHealthDeficit;

        float invertRealSpawnChance = 1 - realSpawnChance;

        realSpawnChance = 1 - Mathf.Pow(invertRealSpawnChance, luckMultiplier);

        if (Random.Range(0f, 100f) <= realSpawnChance)
        {
            //Success !

            //Spawn
            HealthPacks hp= Game.instance.SpawnUnit(healthPackRefab, position);

            //Reset
            spawnChance = 0;

            if (debugPrints)
                Debug.LogWarning("Health pack spawn: " + realSpawnChance + "% chances. Result: Success!");

            return hp;
        }
        else
        {
            if (debugPrints)
                Debug.LogWarning("Health pack spawn: " + realSpawnChance + "% chances. Result: Failed!");

            return null;
        }
    }
}
