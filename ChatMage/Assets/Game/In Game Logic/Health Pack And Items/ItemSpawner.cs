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

    private int killCounter = 0;
    private int commonItemCounter = 0;

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
        Game.Instance.Player.playerStats.OnUnitKilled += OnUnitKilled;
        settings.Init();
    }

    private void OnUnitKilled(Unit unit)
    {
        if (!enabled || unit == null || unit.allegiance != Allegiance.Enemy)
            return;

        killCounter++;

        //Algorithme temporaire
        if (killCounter >= settings.everyXKill)
        {
            SpawnItem(unit.Position);
            killCounter = 0;
        }
    }

    public void SpawnItem(Vector2 at)
    {
        if (settings.commonItems.Count == 0)
        {
            //Il n'y a pas d'item common
            SpawnSpecialItem(at);
            return;
        }
        else if (!settings.itemList.AreThereSpecialItemsAvailable())
        {
            //Il n'y a pas d'item special
            SpawnCommonItem(at);
            return;
        }

        if (commonItemCounter >= settings.specialItemEveryXItem)
        {
            //Il est l'heure d'avoir un special item
            SpawnSpecialItem(at);
            return;
        }
        else
        {
            //Il est l'heure d'avoir un common item
            SpawnCommonItem(at);
            return;
        }
    }

    public void SpawnCommonItem(Vector2 at)
    {
        commonItemCounter++;
        SpawnItem(settings.GainItem(), at);
    }

    public void SpawnSpecialItem(Vector2 at)
    {
        commonItemCounter = 0;
        SpawnItem(settings.GainSpecialItem(), at);
    }

    public void SpawnItem(Item item, Vector2 at)
    {
        ItemPack itemPack = Game.Instance.SpawnUnit(pickupPrefab, at);
        itemPack.SetItem(item);
        itemPack.isPreSpawned = false;
    }
}
