using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : PlayerComponent
{
    [Header("Debug")]
    public List<Item> defaultItems = new List<Item>();

    private List<Item> items = new List<Item>();

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        //Temporaire
        foreach (Item item in defaultItems)
        {
            AddItem(item);
        }
    }

    public override void OnGameReady()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].OnGameReady();
        }
    }

    public override void OnGameStarted()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].OnGameStarted();
        }
    }

    void Update()
    {
        if (Game.instance.gameStarted)
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnGameStarted();
            }
    }

    private void AddItem(Item item)
    {
        items.Add(item);
        item.Init(controller);
    }
}
