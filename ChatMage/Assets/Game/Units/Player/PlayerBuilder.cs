using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuilder : MonoBehaviour
{

    //[Header("Temporaire")]
    //public PlayerController playerPrefab;

    //Assets, filled when loaded
    [Header("Debug Loadout")]
    public DebugLoadout debugLoadout;

    [Header("Current Loadout"), ReadOnly]
    public Car car = null;
    [ReadOnly]
    public Smash smash = null;
    [ReadOnly]
    public List<Item> items = new List<Item>();

    private bool carLoaded = false;
    private bool smashLoaded = false;
    private int loadingItemsCount = 0;

    [System.NonSerialized]
    Action onAllAssetsLoadedCallback = null;

    void UseDebugLoadout()
    {
        car = debugLoadout.car;
        smash = debugLoadout.smash;
        items = new List<Item>(debugLoadout.items);
        onAllAssetsLoadedCallback();
    }

    public void LoadAssets(LoadoutResult loadoutResult, Action callback)
    {
        onAllAssetsLoadedCallback = callback;

        if (loadoutResult == null)
        {
            if (!Application.isEditor)
                Debug.LogError("LOADOUT RESULT IS NULL!");
            UseDebugLoadout();
            return;
        }
        else if (Application.isEditor && debugLoadout != null && debugLoadout.forceUsage)
        {
            Debug.LogWarning("FORCED DEBUG LOADOUT");
            UseDebugLoadout();
            return;
        }

        items = new List<Item>();
        LoadQueue queue = new LoadQueue(callback);

        loadingItemsCount = 0;
        for (int i = 0; i < loadoutResult.itemOrders.Count; i++)
        {
            loadingItemsCount++;
            queue.AddEquipable(loadoutResult.itemOrders[i].equipableName, loadoutResult.itemOrders[i].type, OnItemLoaded);
        }

        if (loadoutResult.carOrder != null)
            queue.AddEquipable(loadoutResult.carOrder.equipableName, loadoutResult.carOrder.type, OnCarLoaded);
        else
            throw new Exception("CAR ORDER IS NULL");

        if (loadoutResult.smashOrder != null)
        {
            queue.AddEquipable(loadoutResult.smashOrder.equipableName, loadoutResult.smashOrder.type, OnSmashLoaded);
        }
        else
        {
            //Si on est dans la v1, on accepte de ne pas avoir de smash
            //Dans la v2, on force le joueur a avoir un smash (il peut rester d�sactiv�, mais il est l�)
            //if (!Game.instance.smashManager.activateV2)
            //    smash = null;
        }

    }

    void OnAnyAssetLoaded()
    {
        if (carLoaded && smashLoaded && loadingItemsCount <= 0)
        {
            onAllAssetsLoadedCallback();
        }
    }

    void OnCarLoaded(Equipable car)
    {
        if (car == null)
            Debug.LogError("Failed to load a car");
        else
            this.car = car as Car;
    }

    void OnSmashLoaded(Equipable smash)
    {
        if (smash == null)
            Debug.LogError("Failed to load a smash");
        else
            this.smash = smash as Smash;
    }

    void OnItemLoaded(Equipable item)
    {
        if (item == null)
            Debug.LogError("Failed to load an item");
        else
            items.Add(item as Item);
    }

    /// <summary>
    /// TODO: Prendre le loadout en parametre
    /// </summary>
    public PlayerController BuildPlayer()
    {
        PlayerController playerController = Instantiate(car.playerPrefab.gameObject).GetComponent<PlayerController>();
        playerController.Init();

        playerController.playerDriver.SetCar(car);
        playerController.playerSmash.SetSmash(smash);
        playerController.playerItems.SetItems(items);

        return playerController;
    }
}
