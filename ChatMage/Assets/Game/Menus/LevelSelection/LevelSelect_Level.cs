using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelSelect
{
    public class LevelSelect_Level : MonoBehaviour
    {
        public Text displayName;
        public Level level;
        public Button button;

        [NonSerialized]
        public RectTransform rectTransform;

        public delegate void LevelSelectEvent(Level level);
        public event LevelSelectEvent onLevelSelected;


        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            button.onClick.AddListener(OnClick);
            displayName.text = level.displayName;
        }

        void OnClick()
        {
            //Click animation !


            //Event
            if (onLevelSelected != null)
                onLevelSelected(level);
        }

        public bool IsUnlocked()
        {
            return level.IsUnlocked();
        }

        // Devrais être fait au début du levelSelect
        public void LoadData()
        {
            level.LoadData();

            gameObject.SetActive(IsUnlocked());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}