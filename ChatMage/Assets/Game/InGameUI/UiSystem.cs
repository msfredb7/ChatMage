using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiSystem : MonoBehaviour {

    public const string SCENENAME = "UI";
    public LevelScript defaultLevelScript;
    public HealthDisplay healthdisplay;
    public PlayerInput playerInputs;

    void Start()
    {
        //if (Scenes.SceneCount() == 1)
        //{
        //    MasterManager.Sync(delegate ()
        //    {
        //        Scenes.Load("Framework", LoadSceneMode.Additive, DebugInit);
        //    });
        //}
    }

    //void DebugInit(Scene scene)
    //{
    //    Framework framework = Scenes.FindRootObject<Framework>(scene);
    //    framework.Init(defaultLevelScript, null);
    //}

    public void Init(PlayerController playerController)
    {
        healthdisplay.Init();
        playerInputs.Init(playerController);
    }
}
