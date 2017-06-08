using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;

public abstract class LevelScript : BaseScriptableObject
{
    public const string WINRESULT_KEY = "winr";

    [InspectorHeader("Info")]
    public string sceneName;
    [InspectorHeader("Base Settings")]
    public bool loseOnPlayerDeath = true;

    public event SimpleEvent onWin;
    public event SimpleEvent onLose;

    public bool IsOver { get { return isOver; } }
    [fsIgnore]
    public bool isOver = false;

    [fsIgnore]
    public InGameEvents events;

    // Init Level Script
    public void Init(System.Action onComplete, InGameEvents events)
    {
        isOver = false;
        Game.instance.onGameReady += GameReady;
        Game.instance.onGameStarted += GameStarted;
        this.events = events;
        events.Init(this);
        OnInit(onComplete);
    }

    // Game Ready for Level Script
    public void GameReady()
    {
        OnGameReady();
    }

    // Game Started for Level Script
    public void GameStarted()
    {
        if (loseOnPlayerDeath)
            Game.instance.Player.vehicle.onDeath += delegate (Unit unit)
            {
                Lose();
            };

        OnGameStarted();
    }

    // Update Level Script
    public void Update()
    {
        OnUpdate();
    }

    public void Win()
    {
        if (IsOver)
            return;

        isOver = true;

        //On enregistre la Win
        GameSaves.instance.SetBool(GameSaves.Type.LevelSelect, WINRESULT_KEY, true);
        GameSaves.instance.SaveData(GameSaves.Type.LevelSelect);

        if (onWin != null)
            onWin();

        OnWin();
    }

    public void Lose()
    {
        if (IsOver)
            return;

        isOver = true;

        if (onLose != null)
            onLose();

        OnLose();
    }

    public abstract void OnInit(System.Action onComplete);
    protected abstract void OnGameReady();
    protected abstract void OnGameStarted();
    protected abstract void OnUpdate();

    public abstract void ReceiveEvent(string message);
    public abstract void OnWin();
    public abstract void OnLose();
}
