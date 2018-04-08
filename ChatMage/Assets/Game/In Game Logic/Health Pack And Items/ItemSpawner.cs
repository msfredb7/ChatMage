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

    public int endlessMaxItemGivenPerStage = 5;

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
        // Fetch items
        if (Game.Instance.levelScript.overrideItems != null)
            settings = Game.Instance.levelScript.overrideItems;

        onUnitSpawnChance = new PseudoRand(settings.gainItemOnKillChance / 100f, settings.gainItemOnKillHardness);
        onItemSpawnChance = new PseudoRand(settings.gainSpecialItemOnItemPackChance / 100f, settings.gainItemOnItemPackHardness);
        Game.Instance.Player.playerStats.OnUnitKilled += OnUnitKilled;
        settings.Init();
    }

    private void OnUnitKilled(Unit unit)
    {
        if (!enabled || unit == null || unit.allegiance != Allegiance.Enemy || unit.GetComponent<NoItemGiver>())
            return;

        if (onUnitSpawnChance.PickResult())
        {
            if (Game.Instance.levelScript is LS_EndlessLevel)
            {
                var endlessLevel = (LS_EndlessLevel)Game.Instance.levelScript;
                if (endlessLevel.GetCharges() <= 0)
                    return;
                else
                {
                    SpawnItem(unit.Position);
                    endlessLevel.UseCharge();
                    return;
                }
            }
            else
                SpawnItem(unit.Position);
        }
    }

    public void SpawnItem(Vector2 at)
    {
        if (onItemSpawnChance.PickResult())
        {
            // Special Item !
            SpawnSpecialItem(at);
        }
        else
        {
            // Common Item !
            SpawnCommonItem(at);
        }
    }

    public void SpawnCommonItem(Vector2 at)
    {
        SpawnItem(false, at);
    }

    public void SpawnSpecialItem(Vector2 at)
    {
        SpawnItem(true, at);
    }

    public void SpawnItem(bool special, Vector2 at)
    {
        ItemPack itemPack = Game.Instance.SpawnUnit(pickupPrefab, at);
        itemPack.SetInfo(settings,special);
        itemPack.isPreSpawned = false;
    }
}
