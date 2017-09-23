using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LoadoutMenu
{
    public class LoadoutProgressPanel : MonoBehaviour
    {
        [Header("Linking")]
        public Image carImage;
        public Image smashImage;
        public Text itemSlotsCounter;
        public RectTransform underliner;

        [Header("Underliner Animations")]
        public float carPos;
        public float smashPos;
        public float itemsPos;
        public float duration;

        [System.NonSerialized]
        private Loadout loadout;

        public void Init(Loadout loadout)
        {
            loadout.onCarChange += OnCarChange;
            loadout.onSmashChange += OnSmashChange;
            loadout.onItemChange += OnItemChange;

            this.loadout = loadout;

            //Force update
            OnSmashChange();
            OnCarChange();
            OnItemChange();

            //Pourrais �tre fait de mani�re plus sexy dans le futur
            if (!Armory.HasAccessToSmash())
                smashImage.rectTransform.parent.gameObject.SetActive(false);
            if (!Armory.HasAccessToItems())
                itemSlotsCounter.rectTransform.parent.gameObject.SetActive(false);
        }

        public void SetTab(LoadoutTab tab, bool animated)
        {
            float newPos = 0;
            switch (tab)
            {
                case LoadoutTab.Car:
                    newPos = carPos;
                    break;
                case LoadoutTab.Smash:
                    newPos = smashPos;
                    break;
                case LoadoutTab.Items:
                    newPos = itemsPos;
                    break;
                default:
                case LoadoutTab.Recap:
                    break;
            }

            underliner.DOKill();

            if (animated)
                underliner.DOAnchorPosX(newPos, duration).SetEase(Ease.InOutSine);
            else
                underliner.anchoredPosition = new Vector2(newPos, underliner.anchoredPosition.y);
        }

        void OnCarChange()
        {
            if (loadout.chosenCar == null)
                carImage.sprite = null;
            else
                carImage.sprite = loadout.chosenCar.preview.icon;
        }

        void OnSmashChange()
        {
            if (loadout.chosenSmash == null)
                smashImage.sprite = null;
            else
                smashImage.sprite = loadout.chosenSmash.preview.icon;
        }

        void OnItemChange()
        {
            UpdateSlotsCount(loadout.chosenItems.Count, loadout.itemSlots);
        }

        void UpdateSlotsCount(int usedSlots, int maxSlots)
        {
            itemSlotsCounter.text = usedSlots + "/" + maxSlots;
        }
    }
}