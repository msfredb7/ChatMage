using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiSystem : MonoBehaviour {

    public const string SCENENAME = "InGameUI";

    public PlayerInput playerInputs;
    public SmashDisplayV2 smashDisplay;
    public SmashDisplayV2_Controller smashDisplayController;
    public DialogDisplay dialogDisplay;
    public BossHealthBarDisplay bossHealthBar;
    public RectTransform stayWithinGameView;

    [HideInInspector]
    public HealthDisplay healthDisplay;
    [HideInInspector]
    public OptionsButton optionsButton;

    [Header("16:9")]
    public HealthDisplay healthdisplayVertical;
    public OptionsButton optionsButtonLow;

    [Header("4:3")]
    public HealthDisplay healthdisplayHorizontal;
    public OptionsButton optionsButtonHigh;
    public GameObject bandeNoir;

    public void Init(PlayerController playerController)
    {
        smashDisplayController.Init(playerController);
        playerInputs.Init(playerController);
        dialogDisplay.onStartDialog += () => playerInputs.Enabled.Lock("dialog");
        dialogDisplay.onEndDialog += () => playerInputs.Enabled.Unlock("dialog");

        float aspect = Game.instance.gameCamera.cam.aspect;

        //Choose hp display
        if (true) //aspect > 1.486f
        {
            //16:9
            healthDisplay = healthdisplayVertical;
            healthdisplayVertical.Init();
            healthdisplayHorizontal.gameObject.SetActive(false);
        }
        else
        {
            //4:3
            //healthDisplay = healthdisplayHorizontal;
            //healthdisplayHorizontal.Init();
            //healthdisplayVertical.gameObject.SetActive(false);
        }

        //Choose options button
        if (true) //aspect > 1.361f
        {
            //16:9
            optionsButton = optionsButtonLow;
            optionsButtonLow.gameObject.SetActive(true);
            optionsButtonHigh.gameObject.SetActive(false);
        }
        else
        {
            //4:3
            //optionsButton = optionsButtonHigh;
            //optionsButtonLow.gameObject.SetActive(false);
            //optionsButtonHigh.gameObject.SetActive(true);
        }

        //On active les bandes noir, du moment que c'est plus carr� que 16:9
        bandeNoir.SetActive(aspect < 1.776);
    }
}
