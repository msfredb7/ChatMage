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
    private DataSaver dataSaver;
    [fsProperty]
    private bool hasBeenCompleted;
    [fsProperty] private bool hasBeenSeen;

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
        hasBeenCompleted = LevelScript.HasBeenCompleted(levelScriptName, dataSaver);
        hasBeenSeen = dataSaver.GetBool(GetCompleteSeenKey());
    }

    [InspectorButton]
    private void ApplyData()
    {
        dataSaver.SetBool(LevelScript.COMPLETED_KEY + levelScriptName, hasBeenCompleted);
        dataSaver.SetBool(GetCompleteSeenKey(), hasBeenSeen);
    }

    private string GetCompleteSeenKey()
    {
        return levelScriptName + SEEN_KEY;
    }
}
