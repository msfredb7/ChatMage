using System;
using System.Collections.Generic;
using UnityEngine;


namespace LevelSelect
{
    public class LevelSelect_Region : MonoBehaviour
    {
        [Header("Les levels DOIVENT etre ancrer sur le mur gauche de la region")]
        public List<LevelSelect_Level> levelItems;
        public event LevelSelect_Level.LevelSelectEvent onLevelSelected;

        [NonSerialized]
        public RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            AddListeners();
        }

        void AddListeners()
        {
            for (int i = 0; i < levelItems.Count; i++)
            {
                levelItems[i].onLevelSelected += OnLevelSelected;
            }
        }

        //On fait remonté l'event jusqu'en haut
        void OnLevelSelected(Level level)
        {
            if (onLevelSelected != null)
                onLevelSelected(level);
        }

        public bool IsUnlocked()
        {
            for (int i = 0; i < levelItems.Count; i++)
            {
                if (levelItems[i].IsUnlocked())
                    return true;
            }
            return false;
        }

        public void LoadData()
        {
            for (int i = 0; i < levelItems.Count; i++)
            {
                levelItems[i].LoadData();
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < levelItems.Count; i++)
            {
                levelItems[i].Hide();
            }
        }

		public bool RegionCompleted()
		{
			for (int i = 0; i < levelItems.Count; i++)
			{
				if (!levelItems[i].IsCompleted())
					return false;
			}
			return true;
		}
    }
}