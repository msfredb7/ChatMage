using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public void LoadScene(string name)
    {
        Scenes.Load(name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
