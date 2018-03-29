using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicSkipper : MonoBehaviour
{
    [Header("References")]
    public GameObject cinematicEnder;
    public GameObject visuals;
    public Image fillImage;

    [Header("Settings"), Suffix("seconds")]
    public float keepDisplayUpFor;
    public float timeNeededToSkip;


    private float lastAttemptTime = float.NegativeInfinity;
    private bool isAttemptingSkip;
    private float cumulatedSkipTime;

    void Update()
    {
        UpdateInput();
        UpdateDisplay();
        UpdateSkipTime();
        UpdateFill();

        if (cumulatedSkipTime >= timeNeededToSkip)
            Skip();
    }

    void Skip()
    {
        cinematicEnder.SetActive(true);
        gameObject.SetActive(false);
    }

    void UpdateSkipTime()
    {
        if (isAttemptingSkip)
            cumulatedSkipTime += Time.deltaTime;
        else
            cumulatedSkipTime = 0;
    }

    void UpdateFill()
    {
        if (isAttemptingSkip)
        {
            fillImage.fillAmount = cumulatedSkipTime / timeNeededToSkip;
        }
        else
        {
            fillImage.fillAmount -= Time.deltaTime * 2;
        }
    }

    void UpdateDisplay()
    {
        if (ShouldDisplay != IsDisplayed)
        {
            visuals.SetActive(ShouldDisplay);
        }
    }

    void UpdateInput()
    {
        if (Input.anyKey || Input.touchCount > 0)
        {
            lastAttemptTime = Time.timeSinceLevelLoad;
            isAttemptingSkip = true;
        }
        else
        {
            isAttemptingSkip = false;
        }
    }

    bool IsDisplayed
    {
        get { return visuals.activeSelf; }
    }

    bool ShouldDisplay
    {
        get { return Time.timeSinceLevelLoad - lastAttemptTime < keepDisplayUpFor; }
    }
}
