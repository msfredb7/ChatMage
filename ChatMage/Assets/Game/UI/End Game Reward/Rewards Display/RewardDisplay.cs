
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndGameReward
{
    public class RewardDisplay : MonoBehaviour
    {
        public Button continueButton;
        public Text levelCleanedText;
        public RewardDisplayItem[] items;

        public void Init(List<EquipablePreview> rewards)
        {
            int i = 0;

            if (rewards != null)
                for (int u = 0; u < rewards.Count; u++)
                {
                    if (i >= items.Length)
                        throw new System.Exception("Ben la, ya trop de rewards.");

                    Debug.LogWarning("TODO: ADD CODE TO UNLOCK ITEM HERE");

                    items[i].Show(rewards[u]);
                    i++;
                }

            continueButton.gameObject.SetActive(true);
            levelCleanedText.gameObject.SetActive(true);
        }
    }
}
