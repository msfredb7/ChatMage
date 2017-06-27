using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarter : MonoBehaviour {

    [System.NonSerialized]
    public BaseTutorial tutorial = null;
    public static MonoBehaviour tutorialScriptObject = null;
    public GameObject canvas;

    public bool Init(BaseTutorial tutorial)
    {
        if (tutorial == null)
            return false;
        if (tutorial.IsComplete())
            return false;
        this.tutorial = tutorial;
        tutorialScriptObject = this;
        LoadQueue queue = new LoadQueue(tutorial.Start);
        tutorial.Begin(canvas, queue);
        return true;
    }
}
