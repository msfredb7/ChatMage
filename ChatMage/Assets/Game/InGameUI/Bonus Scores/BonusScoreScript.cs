using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScoreScript : MonoBehaviour {

    public Text text;

    public int baseValue;

    public string baseText;

    private int currentScore;

    void Start()
    {
        currentScore = baseValue;
        text.text = baseText + currentScore.ToString();
    }

    public void ModifyScore(int value)
    {
        if ((currentScore + value) < 0)
            currentScore = 0;
        else
            currentScore = currentScore + value;
        text.text = baseText + currentScore.ToString();
    }

    public void SetScore(int value)
    {
        currentScore = value;
        text.text = baseText + currentScore.ToString();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
