using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiSystem : MonoBehaviour {

    public const string SCENENAME = "UI";
    public HealthDisplay healthdisplay;
    public PlayerInput playerInputs;

    public void Init(PlayerController playerController)
    {
        healthdisplay.Init();
        playerInputs.Init(playerController);
    }
}
