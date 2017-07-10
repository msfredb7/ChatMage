using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiSystem : MonoBehaviour {

    public const string SCENENAME = "InGameUI";
    public HealthDisplay healthdisplay;
    public PlayerInput playerInputs;
    public SmashDisplay smashDisplay;
    public OptionsButton optionsButton;
    public DialogDisplay dialogDisplay;
    public CanvasGroup gameRelatedGroup;
    public BossHealthBarDisplay bossHealthBar;

    public void Init(PlayerController playerController)
    {
        smashDisplay.Init(playerController);
        healthdisplay.Init();
        playerInputs.Init(playerController);
    }
}
