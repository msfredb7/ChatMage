using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using FullSerializer;
using FullInspector;
using UnityEngine.UI;

public class LS_demoLevelScript : LevelScript
{
    public float enemySpawnDelay = 4f;
    public float hpSpawnDelay = 8f;

    [fsIgnore]
    GameObject countdownUI;
    [fsIgnore]
    GameObject outroUI;
    [fsIgnore]
    GameObject objectiveUI;
    [fsIgnore]
    GameObject tutorialUI;
    [fsIgnore]
    Unit charger;
    [fsIgnore]
    Unit healthPacks;

    //TRES IMPORTANT DE RESET NOS VARIABLE ICI
    protected override void OnGameReady()
    {
        events.LockPlayer();

        events.WinIn(20);

        events.ShowUI(countdownUI).GetComponent<IntroCountdown>().onCountdownOver.AddListener(Game.instance.StartGame);

        // Objective
        GameObject newObjectiveUI = events.ShowUI(objectiveUI);
        newObjectiveUI.GetComponent<ShowObjectives>().AddObjective("Survive 20 seconds !");

        // Tutorial
        GameObject newTutorialUI = events.ShowUIAtLocation(tutorialUI, new Vector2(newObjectiveUI.transform.position.x - 100, newObjectiveUI.transform.position.y - 100));
        events.AddDelayedAction(delegate () {
            newTutorialUI.gameObject.SetActive(true);
            newTutorialUI.GetComponentInChildren<Text>().text = "Here you can see the current objectives of the level. You need to achieve them in order to win! Keep an eye on them because some will update depending on your actions.";
            // tu peux acceder a la fleche avec newTutorialUI.GetComponentInChildren<Image>()
        }, 5);
        events.AddDelayedAction(delegate () {
            Destroy(newTutorialUI);
        }, 15);
    }

    protected override void OnGameStarted()
    {
        events.UnLockPlayer();

        events.SpawnEntitySpreadTime(charger, 20, Waypoint.WaypointType.enemySpawn, 10, true);
        events.SpawnEntitySpreadTime(healthPacks, 20, Waypoint.WaypointType.enemySpawn, 5, true);
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LoadingScreen.IsInTransition)
        {
            if (!isOver)
            {
                isOver = true;
                onObjectiveComplete.Invoke();
            }
        }
    }

    public override void OnEnd()
    {
        if (isOver)
            return;
        isOver = true;

        events.Outro(hasWon, outroUI);
    }

    public override void OnInit(Action onComplete)
    {
        LoadQueue queue = new LoadQueue(onComplete);
        queue.AddEnemy("Charger", (x) => charger = x);
        queue.AddMiscUnit("HealthPacks", (x) => healthPacks = x);
        queue.AddUI("Countdown", (x) => countdownUI = x);
        queue.AddUI("Outro", (x) => outroUI = x);
        queue.AddUI("Objectives", (x) => objectiveUI = x);
        queue.AddUI("Tutorial", (x) => tutorialUI = x);
    }

    public override void ReceiveEvent(string message)
    {
        switch (message)
        {
            default:
                Debug.LogWarning("Demo level script received an unhandled event: " + message);
                break;
        }
    }
}
