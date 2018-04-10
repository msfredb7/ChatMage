using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayExplication : MonoBehaviour {

    public SceneInfo explicationScene;
    public DataSaver endlessTutorialSaver;

    private string alreadyDisplayedKey = "ExplicationDisplayed";

    void Start()
    {
        if (endlessTutorialSaver.GetInt(alreadyDisplayedKey, 0) == 0)
            Display();
    }

	public void Display()
    {
        endlessTutorialSaver.SetInt(alreadyDisplayedKey, 1);
        Scenes.Load(explicationScene);
    }
}
