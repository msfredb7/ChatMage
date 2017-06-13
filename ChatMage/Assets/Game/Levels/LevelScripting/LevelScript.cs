using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;
using LevelScripting;
using DG.Tweening;

public abstract class LevelScript : BaseScriptableObject
{
    public const string WINRESULT_KEY = "winr";

    public enum LosingType { playerDeath, timeOut }
    public enum WinningType { timeOut, enemiesKilled }

    [InspectorHeader("Info")]
    public string sceneName;
    [InspectorHeader("Base Settings")]
    public LosingType[] losingCondition;
    public WinningType[] winningCondition;
    [InspectorTooltip("Follow player on start and disable horizontal bounds")]
    public bool raceFromStart = false;
    public bool usingDefaultIntro = true;
    [InspectorHideIf("usingDefaultIntro")]
    public IntroCountdown countdownUI;
    public bool usingDefaultOutro = true;
    [InspectorHideIf("usingDefaultOutro")]
    public GameResultUI outroUI;
    [InspectorTooltip("No healthpacks during the entire level")]
    public bool hardcoreMode = false;

    [InspectorHeader("In Game Events")]
    public bool useCustomGenericEvents = false;
    [InspectorShowIf("useCustomGenericEvents")]
    public List<EventScripting> events;

    [InspectorHeader("Unit Waves")]
    public List<UnitWave> waves;

    public event SimpleEvent onWin;
    public event SimpleEvent onLose;

    public bool IsOver { get { return isOver; } }
    [fsIgnore]
    public bool isOver = false;

    [fsIgnore]
    public InGameEvents inGameEvent;


    private List<UnitWave> milestoneTriggeredWaves;
    private List<UnitWave> manuallyTriggeredWaves;

    // Init Level Script
    public void Init(System.Action onComplete, InGameEvents events)
    {
        isOver = false;
        Game.instance.onGameReady += GameReady;
        Game.instance.onGameStarted += GameStarted;
        this.inGameEvent = events;
        events.Init(this);
        foreach (EventScripting ev in this.events)
        {
            ev.Init();
            ev.onComplete += EventScriptOnCompleted;
        }
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
        if (IsLoosingWhenPlayerDead())
            Game.instance.Player.vehicle.onDeath += delegate (Unit unit)
            {
                Lose();
            };
          

        StartWaves();

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


    public void ReceiveEvent(string message)
    {
        //Trigger waves ?
        for (int i = 0; i < milestoneTriggeredWaves.Count; i++)
        {
            if(milestoneTriggeredWaves[i].when.name == message)
            {
                LaunchWave(milestoneTriggeredWaves[i]);
                milestoneTriggeredWaves.RemoveAt(i);
                i--;
            }
        }

        OnReceiveEvent(message);
    }

    protected void TriggerWave(string tag)
    {
        //Trigger waves ?
        for (int i = 0; i < manuallyTriggeredWaves.Count; i++)
        {
            if (manuallyTriggeredWaves[i].when.name == tag)
            {
                LaunchWave(manuallyTriggeredWaves[i]);
                manuallyTriggeredWaves.RemoveAt(i);
                i--;
            }
        }
    }

    public abstract void OnInit(System.Action onComplete);
    protected abstract void OnGameReady();
    protected abstract void OnGameStarted();
    protected abstract void OnUpdate();
    public abstract void OnReceiveEvent(string message);
    public abstract void OnWin();
    public abstract void OnLose();

    ///////////////////////////////////////////////////// Loosing/Winning Conditions

    bool IsLoosingWhenPlayerDead()
    {
        foreach (LosingType type in losingCondition)
        {
            if (type == LosingType.playerDeath)
                return true;
        }
        return false;
    }



    ///////////////////////////////////////////////////// Wave queueing
    void StartWaves()
    {
        // Queue tous les waves
        // On les queue � l'envers pour la raison suivante:
        //  ex: Si une wave index 5 doit se launch en m�me temps que la pr�c�dente (index 4)
        //      on doit avoir le temps de placer un listener sur la wave index 4 AVANT de la launch.
        //      Si on fesait l'inverse et que la wave 4 se d�clanchait imm�diatement, on aurait pas eu le temps
        //      de mettre le listener.
        for (int i = waves.Count - 1; i >= 0; i--)
        {
            try
            {
                QueueWave(i);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

    }

    /// <summary>
    /// Returns: Check for next one ?
    /// </summary>
    void QueueWave(int waveIndex)
    {
        UnitWave wave = waves[waveIndex];

        switch (wave.when.type)
        {
            case WaveWhen.Type.At:
                inGameEvent.AddDelayedAction(delegate () { LaunchWave(wave); }, wave.when.time);
                break;


            case WaveWhen.Type.Join:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Join' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    LaunchWave(wave);
                };
                break;


            case WaveWhen.Type.Append:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Append' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    inGameEvent.AddDelayedAction(delegate () { LaunchWave(wave); }, waves[waveIndex - 1].duration);
                };
                break;


            case WaveWhen.Type.AppendPlus:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Append Plus' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    inGameEvent.AddDelayedAction(delegate () { LaunchWave(wave); }, waves[waveIndex - 1].duration + wave.when.time);
                };
                break;


            case WaveWhen.Type.OnMilestone:
                if (milestoneTriggeredWaves == null)
                    milestoneTriggeredWaves = new List<UnitWave>();

                milestoneTriggeredWaves.Add(wave);
                break;


            case WaveWhen.Type.AfterCompletionOfPreviousWave:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'AfterCompletionOfPreviousWave' mode");
                waves[waveIndex - 1].CallbackAfterCompletion(wave.when.finishedRatio, delegate ()
                {
                    LaunchWave(wave);
                });
                break;


            case WaveWhen.Type.OnManualTrigger:
                if (manuallyTriggeredWaves == null)
                    manuallyTriggeredWaves = new List<UnitWave>();

                manuallyTriggeredWaves.Add(wave);
                break;
        }
    }

    void LaunchWave(UnitWave wave)
    {
        wave.Launch(inGameEvent);
    }

    ///////////////////////////////////////////////////// Event scripting

    void EventScriptOnCompleted(EventScripting ev)
    {
        int currentEvent = events.IndexOf(ev);

        // S'il existe un prochain evennement
        if(currentEvent + 1 < events.Count)
        {
            // Si l'evennement complet doit starter le prochain event
            if (ev.eventWhat.startNextEvent)
            {
                // Si le prochain evennement n'est pas deja commence
                if (!events[currentEvent + 1].done)
                {
                    // Si le prochain evennement utilise un temps specifique comme delai
                    if(events[currentEvent + 1].eventWhen.useSpecificTime && !events[currentEvent + 1].eventWhen.invokeOnStart)
                    {
                        Sequence sq = DOTween.Sequence();
                        sq.InsertCallback(events[currentEvent + 1].eventWhen.when, delegate () { events[currentEvent + 1].Launch(); });
                    }
                    else
                        events[currentEvent + 1].Launch();
                }
            }
        }
    }
}
