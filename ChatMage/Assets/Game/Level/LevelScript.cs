using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;

public class LevelScript : BaseScriptableObject
{
    public string displayName;
    public string sceneName;

    public virtual void Init()
    {
        Debug.Log("LevelScript " + displayName + " starting");
    }

    public virtual void Update()
    {
        Debug.Log("LevelScript " + displayName + " updating");
    }

    public virtual void EndLevel()
    {
        Debug.Log("Ending level");
        Game.instance.Quit();
    }
}
