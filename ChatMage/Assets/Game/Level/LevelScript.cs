using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;

public abstract class LevelScript : BaseScriptableObject
{
    public string displayName;
    public string sceneName;

    [fsIgnore]
    public UnityEvent onObjectiveComplete = new UnityEvent();
    [fsIgnore]
    public UnityEvent onObjectiveFailed = new UnityEvent();
    public bool IsOver { get { return isOver; } }
    [fsIgnore]
    public bool isOver = false;


    public void Init(System.Action onComplete)
    {
        isOver = false;
        Game.instance.onGameReady.AddListener(OnGameReady);
        Game.instance.onGameStarted.AddListener(OnGameStarted);
        OnInit(onComplete);
    }

    public abstract void OnInit(System.Action onComplete);

    public abstract void OnGameReady();

    public abstract void OnGameStarted();

    public abstract void Update();
}
