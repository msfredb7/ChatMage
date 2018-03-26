using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// S'occupe de spawn des items quand le joueur tu des ennemies (comme le HealthPackManager le faisait)
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    public ItemPack pickupPrefab;

    public ItemSpawnerSettings settings;

    private PseudoRand onUnitSpawnChance;
    private PseudoRand onItemSpawnChance;

    void Start()
    {
        if (Game.Instance == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("Pas d'instance de Game.");
        }
        else
        {
            Game.Instance.onGameReady += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        onUnitSpawnChance = new PseudoRand(settings.gainItemOnKillChance/100f,settings.gainItemOnKillHardness);
        onItemSpawnChance = new PseudoRand(settings.gainSpecialItemOnItemPackChance/100f,settings.gainItemOnItemPackHardness);
        Game.Instance.Player.playerStats.OnUnitKilled += OnUnitKilled;
        settings.Init();
    }

    private void OnUnitKilled(Unit unit)
    {
        if (!enabled || unit == null || unit.allegiance != Allegiance.Enemy)
            return;

        if (onUnitSpawnChance.PickResult())
        {
            SpawnItem(unit.Position);
        }
    }

    public void SpawnItem(Vector2 at)
    {
        if (onItemSpawnChance.PickResult())
        {
            // Special Item !
            SpawnSpecialItem(at);
        } else
        {
            // Common Item !
            SpawnCommonItem(at);
        }
    }

    public void SpawnCommonItem(Vector2 at)
    {
        SpawnItem(settings.GainItem(), at);
    }

    public void SpawnSpecialItem(Vector2 at)
    {
        SpawnItem(settings.GainSpecialItem(), at);
    }

    public void SpawnItem(Item item, Vector2 at)
    {
        if (item == null)
            return;
        ItemPack itemPack = Game.Instance.SpawnUnit(pickupPrefab, at);
        itemPack.SetItem(item);
        itemPack.isPreSpawned = false;
    }
}
