using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LS_demoLevelScript : LevelScript
{
    [InspectorHeader("Units")]
    public HealthPacks healthPacks;
    public ShielderVehicle shielder;

    [InspectorHeader("UI")]
    public ShowObjectives objectiveUI;

    [InspectorHeader("Tutoriel")]
    public BaseTutorial tutorial;

    public override void OnInit()
    {

    }

    protected override void OnGameReady()
    {
    }

    protected override void OnGameStarted()
    {

    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L) && !IsOver)
        {
            Lose();
        }
        if (Input.GetKeyDown(KeyCode.W) && !IsOver)
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.T) && !IsOver)
        {
            TriggerWaveManually("eersa");
        }
        if (Input.GetKeyDown(KeyCode.Z) && !IsOver)
        {
            Scenes.LoadAsync("Tutorial", LoadSceneMode.Additive, delegate (Scene scene)
            {
                TutorialStarter starter = Scenes.FindRootObject<TutorialStarter>(scene);
                if (starter != null)
                {
                    Debug.Log("Init the tutorial");
                    starter.Init(tutorial);
                }
            });
        }
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "wave complete":
                OnWaveComplete();
                break;
            case "finish":
                Win();
                break;
        }
    }

    private void OnWaveComplete()
    {
        Game.instance.gameCamera.followPlayer = true;
        //Game.instance.gameCamera.canScrollUp = true;
        //Game.instance.gameCamera.canScrollDown = true;
    }

    public override void OnWin()
    {
    }

    public override void OnLose()
    {
    }
}
