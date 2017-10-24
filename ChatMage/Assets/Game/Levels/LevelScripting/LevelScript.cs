using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Manager;
using FullSerializer;
using LevelScripting;
using DG.Tweening;
using GameIntroOutro;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;

public abstract class LevelScript : BaseScriptableObject, IEventReceiver
{
    //public const string WINRESULT_KEY = "winr";
    //public const string HAS_EVER_BEEN_WON = "first-winr";
    public const string COMPLETED_KEY = "cmplt_";
    public static bool HasBeenCompleted(LevelScript lvlScript)
    {
        return HasBeenCompleted(lvlScript.name);
    }
    public static bool HasBeenCompleted(string lvlScriptName)
    {
        return GameSaves.instance.GetBool(GameSaves.Type.Levels, COMPLETED_KEY + lvlScriptName, false);
    }
    public static void MarkAsCompleted(LevelScript lvlScript)
    {
        MarkAsCompleted(lvlScript.name);
    }
    public static void MarkAsCompleted(string lvlScriptName)
    {
        GameSaves.instance.SetBool(GameSaves.Type.Levels, COMPLETED_KEY + lvlScriptName, true);
        GameSaves.instance.SaveData(GameSaves.Type.Levels);
    }










    [InspectorHeader("General Info")]
    public string sceneName;

    [InspectorHeader("Conditions")]
    public GameCondition.BaseWinningCondition[] winningConditions;
    public GameCondition.BaseLosingCondition[] losingConditions;

    [InspectorHeader("Wrap Animations")]
    public BaseIntro introPrefab;
    public BaseWinOutro winOutroPrefab;
    public BaseLoseOutro loseOutroPrefab;


    [InspectorHeader("Base Settings")]
    [InspectorTooltip("No healthpacks during the entire level")]
    public bool noHealthPacks = false;
    public bool followPlayerOnStart = false;

    [InspectorHeader("In Game Events")]
    public bool useCustomGenericEvents = false;
    [InspectorShowIf("useCustomGenericEvents")]
    public List<EventScripting> events;

    [InspectorHeader("Unit Waves")]
    public List<UnitWaveV2> waves;

    [InspectorHeader("Winning Rewards")]
    public GameReward rewards;

    public event SimpleEvent onWin;
    public event SimpleEvent onLose;
    public event StringEvent onEventReceived;

    public bool IsOver { get { return isOver; } }
    [fsIgnore, NotSerialized, NonSerialized]
    public bool isOver = false;

    [fsIgnore, NotSerialized, NonSerialized]
    protected InGameEvents inGameEvents;

    [InspectorHeader("Tutoriel")]
    public bool activateTutorial = false;
    [InspectorShowIf("activateTutorial")]
    public string tutorialAssetName;

    [fsIgnore, NotSerialized]
    private List<UnitWaveV2> eventTriggeredWaves;
    [fsIgnore, NotSerialized]
    private List<UnitWaveV2> manuallyTriggeredWaves;

    protected virtual void ResetData()
    {
        onWin = null;
        onLose = null;
        onEventReceived = null;
        isOver = false;
        inGameEvents = null;
        eventTriggeredWaves = null;
        manuallyTriggeredWaves = null;
    }

    // Init Level Script
    public void Init(System.Action onComplete)
    {
        ResetData();
        Game.instance.onGameReady += GameReady;
        Game.instance.onGameStarted += GameStarted;

        this.inGameEvents = Game.instance.events;

        if (useCustomGenericEvents)
            foreach (EventScripting ev in events)
            {
                ev.Init();
                ev.onComplete += EventScriptOnCompleted;
            }

        OnInit();

        if (onComplete != null)
            onComplete();
    }

    // Game Ready for Level Script
    public void GameReady()
    {
        ApplySettings();

        if (introPrefab != null)
        {
            if (introPrefab.parentType == BaseIntro.ParentType.UnderCanvas)
                inGameEvents.SpawnUnderUI(introPrefab).Play(Game.instance.StartGame);
            else
                inGameEvents.SpawnUnderGame(introPrefab).Play(Game.instance.StartGame);
        }

        if (Armory.HasAccessToSmash())
        {
            Game.instance.smashManager.smashEnabled = true;
            Game.instance.ui.smashDisplay.canBeShown = true;
        }
        else
        {
            Game.instance.smashManager.smashEnabled = false;
            Game.instance.ui.smashDisplay.canBeShown = false;
        }

        OnGameReady();
    }

    // Game Started for Level Script
    public void GameStarted()
    {
        ApplyWLConditions();

        StartWaves();

        if (activateTutorial)
            StartTutorial();

        //Camera follow player ?
        Game.instance.gameCamera.followPlayer = followPlayerOnStart;

        //Les bounds physique qui bloque le joueur
        Game.instance.playerBounds.EnableAll();

        OnGameStarted();
    }

    // Update Level Script
    public void Update()
    {
        OnUpdate();
    }

    [InspectorButton]
    private void Debug_MarkHasCompleted()
    {
        MarkAsCompleted(this);
    }

    public void Win()
    {
        if (IsOver)
            return;

        isOver = true;

        bool wasCompleted = HasBeenCompleted(this);

        rewards.firstWin = !wasCompleted;
        if (!wasCompleted)
        {
            MarkAsCompleted(this);
        }


        if (onWin != null)
            onWin();

        if (winOutroPrefab != null)
        {
            switch (winOutroPrefab.parentType)
            {
                case BaseWinOutro.ParentType.UnderCanvas:
                    inGameEvents.SpawnUnderUI(winOutroPrefab).Play();
                    break;
                case BaseWinOutro.ParentType.UnderCanvasWithinGameView:
                    inGameEvents.SpawnUnderUIWithinGameView(winOutroPrefab).Play();
                    break;
                default:
                case BaseWinOutro.ParentType.UnderGame:
                    inGameEvents.SpawnUnderGame(winOutroPrefab).Play();
                    break;
            }
        }

        OnWin();
    }

    public void Lose()
    {
        if (IsOver)
            return;

        isOver = true;

        //Disable player input
        if (Game.instance.Player != null)
        {
            Game.instance.Player.playerDriver.enableInput = false;
        }

        if (onLose != null)
            onLose();

        if (loseOutroPrefab != null)
        {
            switch (loseOutroPrefab.parentType)
            {
                case BaseLoseOutro.ParentType.UnderCanvas:
                    inGameEvents.SpawnUnderUI(loseOutroPrefab).Play();
                    break;
                case BaseLoseOutro.ParentType.UnderCanvasWithinGameView:
                    inGameEvents.SpawnUnderUIWithinGameView(loseOutroPrefab).Play();
                    break;
                default:
                case BaseLoseOutro.ParentType.UnderGame:
                    inGameEvents.SpawnUnderGame(loseOutroPrefab).Play();
                    break;
            }
        }

        OnLose();
    }

    void StartTutorial()
    {
        if (!string.IsNullOrEmpty(tutorialAssetName))
        {
            Tutorial.TutorialScene.StartTutorial(tutorialAssetName);
        }
    }


    public void ReceiveEvent(string message)
    {
        if (eventTriggeredWaves != null)
            for (int i = 0; i < eventTriggeredWaves.Count; i++)
            {
                if (eventTriggeredWaves[i].when.name == message)
                {
                    LaunchWave(eventTriggeredWaves[i]);
                    if (eventTriggeredWaves[i].when.onlyTriggerOnce)
                    {
                        eventTriggeredWaves.RemoveAt(i);
                        i--;
                    }
                }
            }

        if (events != null)
            for (int i = 0; i < events.Count; i++)
            {
                if (events[i].eventWhen.useMileStone)
                {
                    for (int j = 0; j < events[i].eventWhen.milestoneThatTrigger.Count; j++)
                    {
                        if (events[i].eventWhen.milestoneThatTrigger[j] == message)
                        {
                            events[i].Launch();
                        }
                    }
                }
            }

        OnReceiveEvent(message);

        if (onEventReceived != null)
            onEventReceived(message);
    }

    protected void TriggerWaveManually(string tag)
    {
        for (int i = 0; i < manuallyTriggeredWaves.Count; i++)
        {
            if (manuallyTriggeredWaves[i].when.name == tag)
            {
                LaunchWave(manuallyTriggeredWaves[i]);
                if (manuallyTriggeredWaves[i].when.onlyTriggerOnce)
                {
                    manuallyTriggeredWaves.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void AddEventOnLaunchedUnitWave(string tag, SimpleEvent action)
    {
        for (int i = 0; i < manuallyTriggeredWaves.Count; i++)
        {
            if (manuallyTriggeredWaves[i].when.name == tag)
            {
                manuallyTriggeredWaves[i].onLaunched += action;
            }
        }
    }

    public virtual void OnInit() { }
    protected virtual void OnGameReady() { }
    protected virtual void OnGameStarted() { }
    protected virtual void OnUpdate() { }
    public virtual void OnReceiveEvent(string message) { }
    public virtual void OnWin() { }
    public virtual void OnLose() { }
    public virtual void OnWaveLaunch() { }

    //////////////////////////////////////////////////// Base Settings

    void ApplySettings()
    {
        //Game.instance.SetUnitSnapBorders(horizontalUnitSnap, 0, verticalUnitSnap, 0);
        Game.instance.healthPackManager.enableHealthPackSpawn = !noHealthPacks;
        bool smashAccess = Armory.HasAccessToSmash();
        Game.instance.smashManager.smashEnabled = smashAccess;
        Game.instance.ui.smashDisplay.canBeShown = smashAccess;
    }

    ///////////////////////////////////////////////////// Loosing/Winning Conditions

    /// <summary>
    /// Apply winning and losing conditions
    /// </summary>
    void ApplyWLConditions()
    {
        // Init les losing conditions
        if (losingConditions != null)
            for (int i = 0; i < losingConditions.Length; i++)
            {
                if (losingConditions[i] != null)
                    losingConditions[i].Init(Game.instance.Player, this);
            }

        // Init les winning conditions
        if (winningConditions != null)
            for (int i = 0; i < winningConditions.Length; i++)
            {
                if (winningConditions[i] != null)
                    winningConditions[i].Init(Game.instance.Player, this);
            }
    }

    #region Wave
    ///////////////////////////////////////////////////// Wave queueing
    void StartWaves()
    {
        for (int i = waves.Count - 1; i >= 0; i--)
        {
            waves[i].ResetData(); // pour enlever les listener, etc.
        }

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
        UnitWaveV2 wave = waves[waveIndex];

        switch (wave.when.type)
        {
            case WaveWhen.Type.At:
                inGameEvents.AddDelayedAction(delegate () { LaunchWave(wave); }, wave.when.time);
                break;


            case WaveWhen.Type.Join:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Join' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    LaunchWave(wave);
                };
                break;

            case WaveWhen.Type.JoinPlus:
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    inGameEvents.AddDelayedAction(delegate () { LaunchWave(wave); }, wave.when.time);
                };
                break;

            case WaveWhen.Type.Append:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Append' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    inGameEvents.AddDelayedAction(delegate () { LaunchWave(wave); }, waves[waveIndex - 1].Duration);
                };
                break;


            case WaveWhen.Type.AppendPlus:
                if (waveIndex == 0)
                    throw new System.Exception("Cannot put first wave in 'Append Plus' mode");
                waves[waveIndex - 1].onLaunched += delegate ()
                {
                    inGameEvents.AddDelayedAction(delegate () { LaunchWave(wave); }, waves[waveIndex - 1].Duration + wave.when.time);
                };
                break;


            case WaveWhen.Type.OnLevelEvent:
                if (eventTriggeredWaves == null)
                    eventTriggeredWaves = new List<UnitWaveV2>();

                eventTriggeredWaves.Add(wave);
                break;


            case WaveWhen.Type.OnManualTrigger:
                if (manuallyTriggeredWaves == null)
                    manuallyTriggeredWaves = new List<UnitWaveV2>();

                manuallyTriggeredWaves.Add(wave);
                break;
            case WaveWhen.Type.AppendComplete:
                waves[waveIndex - 1].onComplete += delegate ()
                {
                    LaunchWave(wave);
                };
                break;
            case WaveWhen.Type.AppendCompletePlus:
                waves[waveIndex - 1].onComplete += delegate ()
                {
                    inGameEvents.AddDelayedAction(delegate () { LaunchWave(wave); }, wave.when.time);
                };
                break;
        }
    }

    void LaunchWave(UnitWaveV2 wave)
    {
        if (wave.preLaunchDialog != null)
        {
            Game.instance.ui.dialogDisplay.StartDialog(wave.preLaunchDialog, delegate ()
            {
                wave.LaunchNow(this);
                OnWaveLaunch();
            });
        }
        else
        {
            wave.LaunchNow(this);
            OnWaveLaunch();
        }
    }
    #endregion

    #region Events
    ///////////////////////////////////////////////////// Event scripting

    void EventScriptOnCompleted(EventScripting ev)
    {
        int currentEvent = events.IndexOf(ev);

        // S'il existe un prochain evennement
        if (currentEvent + 1 < events.Count)
        {
            // Si l'evennement complet doit starter le prochain event
            if (ev.eventWhat.startNextEvent)
            {
                // Si le prochain evennement n'est pas deja commence
                if (!events[currentEvent + 1].done)
                {
                    // Si le prochain evennement utilise un temps specifique comme delai
                    if (events[currentEvent + 1].eventWhen.useSpecificTime && !events[currentEvent + 1].eventWhen.invokeOnStart)
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

    #endregion
}
