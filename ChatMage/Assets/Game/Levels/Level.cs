using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class Level : BaseScriptableObject
{
    [InspectorHeader("To Fill")]
    public string displayName;
    public List<Level> previousLevels;
    public string levelScriptName;

    [InspectorHeader("Data"), InspectorMargin(25)]
    [InspectorDisabled, fsProperty]
    private bool hasBeenCompleted;

    public bool HasBeenCompleted
    {
        get { return hasBeenCompleted; }
    }

    private const string SAVE_PREFIX = "lvl";
    private const string COMPLETED_KEY = "cpt";

    public bool IsUnlocked()
    {
        if (previousLevels.Count == 0)
            return true;

        for (int i = 0; i < previousLevels.Count; i++)
        {
            if (previousLevels[i].HasBeenCompleted)
                return true;
        }
        return false;
    }

    public void LoadData()
    {
        string completedCompleteKey = SAVE_PREFIX + name + COMPLETED_KEY;

        if (GameSaves.instance.ContainsBool(GameSaves.Type.LevelSelect, completedCompleteKey))
            hasBeenCompleted = GameSaves.instance.GetBool(GameSaves.Type.LevelSelect, completedCompleteKey);
        else
        {
            hasBeenCompleted = false;
            SaveData();
        }
    }

    private void SaveData()
    {
        string completedCompleteKey = SAVE_PREFIX + name + COMPLETED_KEY;
        GameSaves.instance.SetBool(GameSaves.Type.LevelSelect, completedCompleteKey, hasBeenCompleted);
    }

    public void Complete()
    {
        hasBeenCompleted = true;
        SaveData();
    }
}
