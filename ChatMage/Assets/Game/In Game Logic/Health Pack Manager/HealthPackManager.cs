using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackManager : MonoBehaviour
{
    [Header("Linking")]
    public HealthPacks healthPackPrefab;
    public ArmorPacks armorPackPrefab;
    public bool spawnArmor = false;

    [Header("Debug Setting")]
    public bool debugPrints = false;

    [Header("Luck Settings (in %)")]
    public float lerpUpdateRate = 0.001f;
    public float lerpPerKill = 0.1f;
    public float lerpCeiling = 50;
    public float luckIncreaseByHealthDeficit = 4f;
    public float luckMultiplier = 1;

    public bool enableHealthPackSpawn = true;
    public bool replaceWithArmorPack = false;

    [ReadOnly, Header("Live Data")]
    public float spawnChance = 0;

    PlayerController player;

    public void Init(PlayerController controller)
    {
        controller.playerStats.onUnitKilled += PlayerStats_onUnitKilled;
        player = controller;
        spawnArmor = false;
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
        if (unit.allegiance != Allegiance.Enemy)
            return;

        //On n'augmente pas si le joueur est full hp
        if (player.playerStats.health >= player.playerStats.health.MAX)
            return;

        //Increase
        spawnChance = Mathf.Lerp(spawnChance, lerpCeiling, lerpPerKill);

        //Try to spawn health pack
        TryToSpawnHealthPack(unit.Position);
    }

    public void TryToSpawnHealthPack(Vector2 position)
    {
        if (player == null)
            return;

        if (!enableHealthPackSpawn)
            return;

        float realSpawnChance = spawnChance
            + (player.playerStats.health.MAX - player.playerStats.health) * luckIncreaseByHealthDeficit;

        float invertRealSpawnChance = 1 - realSpawnChance;

        realSpawnChance = 1 - Mathf.Pow(invertRealSpawnChance, luckMultiplier);

        if (Random.Range(0f, 100f) <= realSpawnChance)
        {
            //Success !

            //Spawn
            if(spawnArmor)
                Game.instance.SpawnUnit(armorPackPrefab, position);
            else
                Game.instance.SpawnUnit(healthPackPrefab, position);

            //Reset
            spawnChance = 0;

            if (debugPrints)
                Debug.LogWarning("Health pack spawn: " + realSpawnChance + "% chances. Result: Success!");
        }
        else
        {
            if (debugPrints)
                Debug.LogWarning("Health pack spawn: " + realSpawnChance + "% chances. Result: Failed!");
        }
    }
}
