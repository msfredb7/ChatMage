using CCC.Manager;
using DG.Tweening;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorial : BaseScriptableObject
{
    public class TutorialEvent
    {
        [InspectorTooltip("Who's the script with the event")]
        public string scriptName;
        [InspectorTooltip("Function name to cast the event")]
        public string functionName;
        [InspectorHeader("Use a specific time to start the event")]
        public bool useSpecificTime = false;
        [InspectorShowIf("useSpecificTime")]
        public float when = 0;
        [InspectorHeader("Use a milestone to start the event")]
        public bool useMileStone = false;
        [InspectorShowIf("useMileStone")]
        public List<string> milestoneThatTrigger = new List<string>();
        [InspectorTooltip("If at false, time counter won't start from" +
            "beginning but from where it was trigger (example : from outside)")]
        public bool invokeOnGameStarted = true;

    }

    public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();
    
    protected GameObject spotLight;
    protected GameObject inputBlocker;
    protected GameObject buttonPrefab;
    protected GameObject tutorialInfoDisplay;

    protected GameObject currentCanvas;
    protected GameObject currentSpotLight;
    protected GameObject currentInputBlocker;
    protected GameObject currentButtonPrefab;
    protected GameObject currentTutorialInfoDisplay;

    /// <summary>
    /// Utiliser pour l'initialisation des variables
    /// </summary>
	public virtual void Begin(GameObject canvas)
    {
        LoadQueue queue = new LoadQueue(Start);
        queue.AddUI("Spotlight", (x) => spotLight = x);
        queue.AddUI("InputDisabler", (x) => inputBlocker = x);
        queue.AddUI("ReplacementButton", (x) => buttonPrefab = x);
        queue.AddUI("InfoDisplay", (x) => tutorialInfoDisplay = x);
        currentCanvas = canvas;
    }

    /// <summary>
    /// Debut du tutoriel
    /// </summary>
    protected virtual void Start()
    {
        Sequence sq = DOTween.Sequence();

        for (int i = 0; i < tutorialEvents.Count; i++)
        {
            if (tutorialEvents[i].invokeOnGameStarted || tutorialEvents[i].useSpecificTime)
                Execute(tutorialEvents[i], null);
        }
    }

    /// <summary>
    /// Chaque update du jeu
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// Fin de la partie
    /// </summary>
    public virtual void End()
    {
        TutorialStarter.tutorialScriptObject.StopAllCoroutines();
        if(currentTutorialInfoDisplay != null) // Si on arretait la partie avant d'avoir display un seul text dans le tutoriel, il y avait erreur ici
            currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
        Scenes.UnloadAsync("Tutorial");
    }

    public void Execute(TutorialEvent tutorialEvent, Sequence sequence){
        Type thisType = Type.GetType(tutorialEvent.scriptName);
        MethodInfo theMethod = thisType.GetMethod(tutorialEvent.functionName);

        if(sequence == null)
            theMethod.Invoke(this, null);
        //else
            //sequence.InsertCallback(tutorialEvent.when, delegate () { theMethod.Invoke(Game.instance.currentLevel, null); });
    }

    /// <summary>
    /// Le spotlight se déplacera à la position donnant une impression d'emphase
    /// </summary>
    protected void FocusSpotLight(Vector2 position)
    {
        Time.timeScale = 0;
        if (currentSpotLight == null)
            currentSpotLight = Instantiate(spotLight, currentCanvas.transform);

        currentSpotLight.transform.position = position;
        currentSpotLight.GetComponent<SpotlightAnimation>().Init(currentCanvas);
    }

    /// <summary>
    /// Le spotlight se déplacera à la position donnant une impression d'emphase
    /// </summary>
    protected void FocusSpotLight(Vector2 position, bool blockInput = false, bool stopTime = false)
    {
        if (stopTime)
            Time.timeScale = 0;
        if (currentSpotLight == null)
        {
            if (currentCanvas != null)
                currentSpotLight = Instantiate(spotLight, currentCanvas.transform);
            else
                return;
        }

        currentSpotLight.transform.position = position;
        currentSpotLight.GetComponent<SpotlightAnimation>().Init(currentCanvas);
        if (blockInput)
        {
            if (currentInputBlocker == null)
                currentInputBlocker = Instantiate(inputBlocker, currentCanvas.transform);
            currentInputBlocker.SetActive(true);
        }
    }

    /// <summary>
    /// Le spotlight disparait
    /// </summary>
    protected void DeFocusSpotLight(bool resetTime, Action onComplete)
    {
        currentSpotLight.GetComponent<SpotlightAnimation>().Close(delegate() {
            if (resetTime)
                Time.timeScale = 1;
            currentInputBlocker.SetActive(false);
            onComplete.Invoke();
        });
    }

    /// <summary>
    /// Emphase sur l'input à un élément du UI, tout autre input est annuler
    /// </summary>
    protected void FocusInput(GameObject obj, bool stopTime = true)
    {
        FocusSpotLight(obj.transform.position, true, stopTime);

        if (currentButtonPrefab == null)
        {
            if (currentCanvas != null)
                    currentButtonPrefab = Instantiate(buttonPrefab, currentCanvas.transform);
            else
                return;
        }

        currentButtonPrefab.transform.position = obj.transform.position;
        currentButtonPrefab.SetActive(true);
        currentButtonPrefab.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            DeFocusSpotLight(stopTime, delegate () {
                obj.GetComponent<Button>().onClick.Invoke();
                if (currentTutorialInfoDisplay != null)
                    currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
                currentInputBlocker.SetActive(false);
                currentButtonPrefab.SetActive(false);
            });
        });
    }

    protected void ShowInfo(string text)
    {
        if (currentTutorialInfoDisplay == null)
        {
            if (currentCanvas != null)
                    currentTutorialInfoDisplay = Instantiate(tutorialInfoDisplay, currentCanvas.transform);
                else
                return;
        }

        TutorialInfo info = currentTutorialInfoDisplay.GetComponent<TutorialInfo>();
        info.DisplayInfo(text);
        DelayManager.LocalCallTo(delegate ()
        {
            info.OnEnd();
        }, info.coolDown, TutorialStarter.tutorialScriptObject);
    }
}
