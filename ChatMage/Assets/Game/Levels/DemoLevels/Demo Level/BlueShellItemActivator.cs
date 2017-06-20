using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueShellItemActivator : Activator
{
    public override void Activate()
    {
        Debug.Log("Button Activated");
        (Scenes.FindRootObject<PlayerBuilder>(SceneManager.GetSceneByName("Framework")).items[0] as ITM_BlueShell).enable = true;
    }
}
