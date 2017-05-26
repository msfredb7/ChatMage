using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour {

    public string text;
    public int currentResult;
    public Text uitext;
    public bool resultExist;

    public void SetObjective(string text)
    {
        this.text = text;
        resultExist = false;
    }

    public void SetObjective(string text, int currentResult)
    {
        this.text = text;
        this.currentResult = currentResult;
        resultExist = true;
    }

    public void UpdateObjective()
    {
        if (resultExist)
        {
            UpdateUIWithResult();
        } else
        {
            UpdateUIW();
        }
    }

    public void UpdateUIWithResult()
    {
        uitext.text = text + currentResult;
    }

    public void UpdateUIW()
    {
        uitext.text = text;
    }
}
