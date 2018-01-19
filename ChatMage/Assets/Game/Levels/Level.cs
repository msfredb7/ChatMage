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
    [fsProperty]
    private bool hasBeenCompleted;
    [fsProperty]
    private bool hasBeenSeen;

    public bool HasBeenCompleted
    {
        get { return hasBeenCompleted; }
    }
    public bool HasBeenSeen
    {
        get { return hasBeenSeen; }
        set { hasBeenSeen = value; ApplyData(); }
    }

    private const string SEEN_KEY = "_seen";

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
        hasBeenCompleted = LevelScript.HasBeenCompleted(levelScriptName);
        hasBeenSeen = DataSaver.instance.GetBool(DataSaver.Type.Levels, GetCompleteSeenKey());
    }

    [InspectorButton]
    private void ApplyData()
    {
		DataSaver.instance.SetBool(DataSaver.Type.Levels, LevelScript.COMPLETED_KEY + levelScriptName, hasBeenCompleted);
		DataSaver.instance.SetBool(DataSaver.Type.Levels, GetCompleteSeenKey(), hasBeenSeen);
    }

    private string GetCompleteSeenKey()
    {
        return levelScriptName + SEEN_KEY;
    }
}
