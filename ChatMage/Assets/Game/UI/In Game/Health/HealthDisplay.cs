using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class HealthDisplay : CanvasGroupBehaviour
{
    [Header("Health display")]
    public GameObject hearthCountainer;
    public GameObject hearth;

    private List<HeartItem> hearts = new List<HeartItem>();
    private PlayerStats playerStats;

    public void Init()
    {
        playerStats = Game.instance.Player.playerStats;
        playerStats.health.onSet.AddListener(UpdateHearts);
        playerStats.armor.onSet.AddListener(UpdateArmor);
        playerStats.health.onMaxSet.AddListener(UpdateAll);

        // Set initial HP
        UpdateAll(-1);

        HideInstant();

        Game.instance.onGameStarted += Show;
    }

    void UpdateAll(int bidon)
    {
        UpdateHearts(playerStats.health);
        UpdateArmor(playerStats.armor);
    }

    void UpdateHearts(int hp)
    {
        int i = 0;
        for (; i < hp; i++)
        {
            SetHeartAs(i, HeartItem.HeartType.Full);
        }
        for (; i < playerStats.health.MAX; i++)
        {
            SetHeartAs(i, HeartItem.HeartType.Empty);
        }
    }

    void UpdateArmor(int armor)
    {
        int i = playerStats.health.MAX;
        for (; i < playerStats.health.MAX + armor; i++)
        {
            SetHeartAs(i, HeartItem.HeartType.Armor);
        }
        for (; i < hearts.Count; i++)
        {
            hearts[i].Hide();
        }
    }

    private void SetHeartAs(int index, HeartItem.HeartType type)
    {
        while (index >= hearts.Count)
        {
            NewHeart();
        }

        hearts[index].Display(type);
    }

    private HeartItem NewHeart()
    {
        HeartItem newHeart = Instantiate(hearth, hearthCountainer.transform).GetComponent<HeartItem>();
        hearts.Add(newHeart);
        return newHeart;
    }
}
