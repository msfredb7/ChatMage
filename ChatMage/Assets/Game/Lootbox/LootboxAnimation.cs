using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using CCC.UI;
using CCC.Manager;

public class LootboxAnimation : WindowAnimation {

    public GameObject background;
    public GameObject lootboxIcon;
    public GameObject rewardCountainer;
    public GameObject rewardIconPrefab;
    public GameObject goldifyButton;

    public SimpleEvent goldifyEvent;
    public SimpleEvent lootboxOpeningEvent;

    public bool lootboxOpened;

    void Start()
    {
        goldifyButton.GetComponent<Button>().interactable = true;
        goldifyButton.GetComponent<Button>().onClick.AddListener(OnGoldify);
        lootboxIcon.GetComponent<Button>().onClick.AddListener(ShowRewards);
        rewardCountainer.SetActive(false);
        lootboxOpened = false;
    }

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

    void ShowRewards()
    {
        if (!lootboxOpened)
        {
            Debug.Log("Opening Lootbox !");
            lootboxOpeningEvent.Invoke();
            rewardCountainer.SetActive(true); // TODO: faire une meilleur animation
            goldifyButton.GetComponent<Button>().interactable = false;
            DelayManager.CallTo(delegate () { Close(delegate() { Destroy(gameObject); });  }, 2.5f);
            lootboxOpened = true;
        }
    }

    void OnGoldify()
    {
        Debug.Log("Goldify");
        goldifyEvent.Invoke();
    }
}
