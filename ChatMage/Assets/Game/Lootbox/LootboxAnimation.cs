using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using CCC.UI;

public class LootboxAnimation : WindowAnimation {

    public GameObject background;
    public GameObject lootboxIcon;
    public GameObject rewardCountainer;
    public GameObject rewardIconPrefab;

    void Update()
    {
        background.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 1), 1);
    }

    public void AddRewards(List<EquipablePreview> equipables)
    {
        for (int i = 0; i < equipables.Count; i++)
        {
            GameObject newReward = Instantiate(rewardIconPrefab, rewardCountainer.transform);
            newReward.GetComponent<Image>().sprite = equipables[i].icon;
        }
    }
}
