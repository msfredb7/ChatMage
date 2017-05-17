using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour {

	public void LoadLevel(string name)
    {
        Scenes.Load(name);

    }
}
