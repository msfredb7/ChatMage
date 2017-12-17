using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// S'occupe de spawn des items quand le joueur tu des ennemies (comme le HealthPackManager le faisait)
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    public ItemPack pickupPrefab;
    public List<Item> commonItemsReferences = new List<Item>();
    public List<Item> specialItemsReferences = new List<Item>();

    [Header("Algorithme simple TEMPORAIRE")]
    public int everyXKills = 5;
    public int specialEveryXItem = 3;

    private int killCounter = 0;
    private int commonItemCounter = 0;

    void Start()
    {
        if (Game.instance == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("Pas d'instance de Game.");
        }
        else
        {
            Game.instance.onGameReady += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.instance.Player.playerStats.onUnitKilled += OnUnitKilled;
    }

    private void OnUnitKilled(Unit unit)
    {
        if (!enabled || unit == null || unit.allegiance != Allegiance.Enemy)
            return;

        killCounter++;

        //Algorithme temporaire
        if (killCounter >= everyXKills)
        {
            SpawnItem(unit.Position);
            killCounter = 0;
        }
    }

    public void SpawnItem(Vector2 at)
    {
        if (commonItemsReferences.Count == 0)
        {
            //Il n'y a pas d'item common
            SpawnSpecialItem(at);
        }
        else if (specialItemsReferences.Count == 0)
        {
            //Il n'y a pas d'item special
            SpawnCommonItem(at);
        }

        if (commonItemCounter >= specialEveryXItem)
        {
            //Il est l'heure d'avoir un special item
            SpawnSpecialItem(at);
        }
        else
        {
            //Il est l'heure d'avoir un common item
            SpawnCommonItem(at);
        }
    }

    public void SpawnCommonItem(Vector2 at)
    {
        SpawnItem(commonItemsReferences.PickRandom(), at);
    }

    public void SpawnSpecialItem(Vector2 at)
    {
        SpawnItem(specialItemsReferences.PickRandom(), at);
    }

    public void SpawnItem(Item item, Vector2 at)
    {
        Game.instance.SpawnUnit(pickupPrefab, at).SetItem(item);
    }
}
