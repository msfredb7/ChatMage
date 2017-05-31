using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTutorialInfo : TutorialInfo
{
    public GameObject textDisplay;

    public override void DisplayInfo(string text)
    {
        textDisplay.SetActive(true);
        textDisplay.GetComponent<Text>().text = text;
    }

    public override void OnEnd()
    {
        textDisplay.SetActive(false);
    }
}
