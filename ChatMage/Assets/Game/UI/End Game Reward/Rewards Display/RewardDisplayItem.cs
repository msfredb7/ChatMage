using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndGameReward
{
    public class RewardDisplayItem : MonoBehaviour
    {
        public Image itemImage;
        public Image goldImage;
        public Text topText;
        public Image duplicateImage;

        public void Show(int gold, bool bonus)
        {
            if (bonus)
            {
                topText.text = "First win: +" + gold + "g";
            }
            else
            {
                topText.text = "Base reward: +" + gold + "g";
            }
            goldImage.enabled = true;
            gameObject.SetActive(true);
        }

        public void Show(EquipablePreview newEquipable)
        {
            topText.text = newEquipable.displayName;
            itemImage.sprite = newEquipable.icon;
            itemImage.enabled = true;
            gameObject.SetActive(true);
        }

        public void Show(EquipablePreview doublouEquipable, int gold)
        {
            topText.text = "Duplicate: +" + gold + "g";

            itemImage.sprite = doublouEquipable.icon;
            itemImage.enabled = true;
            duplicateImage.enabled = true;
            gameObject.SetActive(true);
        }
    }
}
