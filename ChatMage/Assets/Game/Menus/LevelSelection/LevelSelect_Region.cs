using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelSelect_Region : MonoBehaviour
{
    public List<LevelSelect_Level> levelItems;
    public event LevelSelect_Level.LevelSelectEvent onLevelSelected;

    void Start()
    {
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

    /// <summary>
    /// Returns true if the level was indeed in this list
    /// </summary>
    public bool SetAsCompleted(string levelName)
    {
        for (int i = 0; i < levelItems.Count; i++)
        {
            if(levelItems[i].level.name == levelName)
            {
                levelItems[i].level.Complete();
                return true;
            }
        }
        return false;
    }
}