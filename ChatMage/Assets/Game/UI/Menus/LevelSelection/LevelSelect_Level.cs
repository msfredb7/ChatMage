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

        // First Apparition Animation
        [HideInInspector]
        public bool hasBeenSeen;
        private string hasBeenSeenKey = "_seen";

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            button.onClick.AddListener(OnClick);
            displayName.text = level.displayName;
        }

        void Start()
        {
            if (GameSaves.instance.ContainsBool(GameSaves.Type.Levels, level.levelScriptName + hasBeenSeenKey))
                hasBeenSeen = GameSaves.instance.GetBool(GameSaves.Type.Levels, level.levelScriptName + hasBeenSeenKey);
            /*else
                GameSaves.instance.SetBool(GameSaves.Type.Levels, hasBeenSeenKey + level.levelScriptName, false);*/
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

			gameObject.SetActive(IsUnlocked() && (level.previousLevels.Count > 0 ? level.previousLevels[0].HasBeenSeen : true));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void MarkAsSeen()
        {
            GameSaves.instance.SetBool(GameSaves.Type.Levels, level.levelScriptName + hasBeenSeenKey, true);
            hasBeenSeen = true;
        }
    }
}