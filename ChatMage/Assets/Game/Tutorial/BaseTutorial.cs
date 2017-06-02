using CCC.Manager;
using FullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorial : BaseScriptableObject
{
    private GameObject canvas;
    private GameObject spotLight;
    private GameObject inputBlocker;
    private GameObject buttonPrefab;
    private GameObject tutorialInfoDisplay;

    private GameObject currentCanvas;
    private GameObject currentSpotLight;
    private GameObject currentInputBlocker;
    private GameObject currentButtonPrefab;
    private GameObject currentTutorialInfoDisplay;

    /// <summary>
    /// Utiliser pour l'initialisation des variables
    /// </summary>
	public virtual void Begin()
    {
        LoadQueue queue = new LoadQueue(Start);
        queue.AddUI("Spotlight", (x) => spotLight = x);
        queue.AddUI("BasicCanvas", (x) => canvas = x);
        queue.AddUI("InputDisabler", (x) => inputBlocker = x);
        queue.AddUI("ReplacementButton", (x) => buttonPrefab = x);
        queue.AddUI("InfoDisplay", (x) => tutorialInfoDisplay = x);
    }

    /// <summary>
    /// Debut du tutoriel
    /// </summary>
    protected virtual void Start()
    {
        currentCanvas = Instantiate(canvas);
    }

    /// <summary>
    /// Chaque update du jeu
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Fin de la partie
    /// </summary>
    public virtual void End()
    {
        TutorialStarter.tutorialScriptObject.StopAllCoroutines();
        currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
        Scenes.UnloadAsync("Tutorial");
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
    protected void DeFocusSpotLight(bool resetTime)
    {
        if (resetTime)
            Time.timeScale = 1;
        currentSpotLight.GetComponent<SpotlightAnimation>().Close();
        currentInputBlocker.SetActive(false);
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
            obj.GetComponent<Button>().onClick.Invoke();
            if (currentTutorialInfoDisplay.GetComponent<TutorialInfo>() != null)
                currentTutorialInfoDisplay.GetComponent<TutorialInfo>().OnEnd();
            DeFocusSpotLight(stopTime);
            currentInputBlocker.SetActive(false);
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
