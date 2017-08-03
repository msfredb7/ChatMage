using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicSettings : BaseCinematicSettings
{
    public string nextSceneName;
    public SceneMessage nextSceneMessage;
    public bool waitForNextSceneSetup = false;
}
