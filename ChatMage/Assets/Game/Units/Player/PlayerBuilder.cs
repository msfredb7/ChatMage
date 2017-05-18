﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuilder : MonoBehaviour
{

    [Header("Temporaire")]
    public PlayerController playerPrefab;

    //Assets, filled when loaded
    [Header("Debug Loadout")]
    public Car car = null;
    public Smash smash = null;
    public List<Item> items = new List<Item>();

    private bool carLoaded = false;
    private bool smashLoaded = false;
    private int loadingItemsCount = 0;

    [System.NonSerialized]
    UnityAction onAllAssetsLoadedCallback = null;

    public void LoadAssets(LoadoutResult loadoutResult, UnityAction callback)
    {
        onAllAssetsLoadedCallback = callback;
        if (loadoutResult == null)
        {
            onAllAssetsLoadedCallback();
            Debug.LogWarning("Debug loadout");
            return;
        }

        items = new List<Item>();

        loadingItemsCount = 0;
        for (int i = 0; i < loadoutResult.itemOrders.Count; i++)
        {
            loadingItemsCount++;
            ResourceLoader.LoadEquipableAsync(loadoutResult.itemOrders[i].equipableName, loadoutResult.itemOrders[i].type, OnItemLoaded);
        }
        ResourceLoader.LoadEquipableAsync(loadoutResult.carOrder.equipableName, loadoutResult.carOrder.type, OnCarLoaded);
        ResourceLoader.LoadEquipableAsync(loadoutResult.smashOrder.equipableName, loadoutResult.smashOrder.type, OnSmashLoaded);
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
        carLoaded = true;
        OnAnyAssetLoaded();
    }

    void OnSmashLoaded(Equipable smash)
    {
        if (smash == null)
            Debug.LogError("Failed to load a smash");
        else
            this.smash = smash as Smash;
        smashLoaded = true;
        OnAnyAssetLoaded();
    }

    void OnItemLoaded(Equipable item)
    {
        if (item == null)
            Debug.LogError("Failed to load an item");
        else
            items.Add(item as Item);
        loadingItemsCount--;
        OnAnyAssetLoaded();
    }

    /// <summary>
    /// TODO: Prendre le loadout en parametre
    /// </summary>
    public Vehicle BuildPlayer()
    {
        PlayerController playerController = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();
        playerController.Init();

        playerController.playerDriver.SetCar(car);
        playerController.playerSmash.SetSmash(smash);
        playerController.playerItems.SetItems(items);

        return playerController.GetComponent<Vehicle>();
    }
}
