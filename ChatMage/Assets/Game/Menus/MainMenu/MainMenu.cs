using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }
	public void LoadScene(string name)
    {
        LoadingScreen.TransitionTo(name, null);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
