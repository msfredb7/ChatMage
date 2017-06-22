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
        [InspectorHeader("Will start this event when the other one is over")]
        public bool startAfterAnEvent = false;
        [InspectorShowIf("startAfterAnEvent")]
        public int tutorialEventIndex;
        public SimpleEvent onComplete;

        [HideInInspector, NonSerialized]
        public bool alreadyExecute = false;

        public void OnComplete() {
            if(onComplete != null)
            {
                onComplete.Invoke();
            }
            Debug.Log(functionName + "Event Complete");
        }
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
	public virtual void Begin(GameObject canvas, LoadQueue queue)
    {
        queue.AddUI("Spotlight", (x) => spotLight = x);
        queue.AddUI("InputDisabler", (x) => inputBlocker = x);
        queue.AddUI("ReplacementButton", (x) => buttonPrefab = x);
        queue.AddUI("InfoDisplay", (x) => tutorialInfoDisplay = x);
        currentCanvas = canvas;
    }

    /// <summary>
    /// Debut du tutoriel
    /// </summary>
    public virtual void Start()
    {
        // Avant meme de commencer a faire les events, on doit s'assurer que l'enchainement se fera comme il faut
        for (int i = 0; i < tutorialEvents.Count; i++)
        {
            TutorialEvent currentTutorial = tutorialEvents[i];
            if (currentTutorial.startAfterAnEvent)
            {
                tutorialEvents[currentTutorial.tutorialEventIndex].onComplete += delegate () { Execute(currentTutorial, true); };
                // Never Execute It Again
                currentTutorial.alreadyExecute = true;
            }
                
        }

        // On peut ensuite commencer les events qui sont start au debut
        for (int i = 0; i < tutorialEvents.Count; i++)
        {
            if (tutorialEvents[i].alreadyExecute)
                continue;
            if (tutorialEvents[i].invokeOnGameStarted || tutorialEvents[i].useSpecificTime)
                Execute(tutorialEvents[i], true);
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
        if (currentTutorialInfoDisplay != null) // Si on arretait la partie avant d'avoir display un seul text dans le tutoriel, il y avait erreur ici
            currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
        Scenes.UnloadAsync("Tutorial");
    }

    public void Execute(TutorialEvent tutorialEvent, bool useTime)
    {
        tutorialEvent.alreadyExecute = true;

        // Find the Action/Method to Invoke
        Type thisType = Type.GetType(tutorialEvent.scriptName);
        MethodInfo theMethod = thisType.GetMethod(tutorialEvent.functionName);

        // Gere le temps
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        
        if (useTime && tutorialEvent.useSpecificTime)
            sequence.InsertCallback(tutorialEvent.when, delegate () {
                theMethod.Invoke(Game.instance.currentLevel.tutorial, null);
                tutorialEvent.OnComplete();
            });
        else
        {
            theMethod.Invoke(Game.instance.currentLevel.tutorial, null);
            tutorialEvent.OnComplete();
        }
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

        currentSpotLight.GetComponent<RectTransform>().anchoredPosition = position;
        currentSpotLight.GetComponent<SpotlightAnimation>().Init(currentCanvas);
        if (blockInput)
        {
            if (currentInputBlocker == null)
                currentInputBlocker = Instantiate(inputBlocker, currentCanvas.transform);
            currentInputBlocker.SetActive(true);
        }
    }

    /// <summary>
    /// Le spotlight se déplacera à la position donnant une impression d'emphase
    /// </summary>
    protected void FocusSpotLight(bool blockInput = false, bool stopTime = false)
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
        
        currentSpotLight.GetComponent<SpotlightAnimation>().Init(currentCanvas);
        if (blockInput)
        {
            if (currentInputBlocker == null)
                currentInputBlocker = Instantiate(inputBlocker, currentCanvas.transform);
            currentInputBlocker.SetActive(true);
        }
    }

    /// <summary>
    /// Le spotlight se déplacera à la position donnant une impression d'emphase
    /// </summary>
    protected void MoveSpotlight(Vector2 position, Action onComplete, float speed = 1)
    {
        Tweener animation = currentSpotLight.transform.DOMove(position, speed).SetUpdate(true);
        animation.OnComplete(delegate () { onComplete.Invoke(); });
    }

    /// <summary>
    /// Le spotlight disparait
    /// </summary>
    protected void DeFocusSpotLight(bool resetTime, Action onComplete)
    {
        currentSpotLight.GetComponent<SpotlightAnimation>().Close(delegate ()
        {
            if (resetTime)
                Time.timeScale = 1;
            currentInputBlocker.SetActive(false);
            if(onComplete != null)
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

        currentSpotLight.GetComponent<RectTransform>().anchoredPosition = obj.transform.position;
        currentButtonPrefab.SetActive(true);
        currentButtonPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
        currentButtonPrefab.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            DeFocusSpotLight(stopTime, delegate ()
            {
                obj.GetComponent<Button>().onClick.Invoke();
                if (currentTutorialInfoDisplay != null)
                    currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
                currentInputBlocker.SetActive(false);
                currentButtonPrefab.SetActive(false);
            });
        });
    }

    protected void ShowInfo(string text, float delay)
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
        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.InsertCallback(delay, delegate ()
        {
            info.OnEnd();
        });
    }
}
