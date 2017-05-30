using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : PlayerComponent
{
    public List<Item> items = new List<Item>();

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
    }

    public void SetItems(List<Item> items)
    {
        foreach (Item item in items)
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
