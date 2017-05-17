using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;

public class LevelScript : BaseScriptableObject
{
    public string displayName;
    public string sceneName;
    [SerializeField]
    private LevelBehavior levelBehavior;

    public void Init()
    {
        Debug.Log("LevelScript" + displayName + " starting");
        levelBehavior.OnBegin(this);
        levelBehavior.onEnding.AddListener(EndLevel);
    }

    public void Update()
    {
        Debug.Log("LevelScript" + displayName + " updating");
        levelBehavior.OnUpdate();
    }

    public void EndLevel()
    {
        Debug.Log("Ending level");
        Game.instance.Quit();
    }
}
