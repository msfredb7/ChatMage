using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiSystem : MonoBehaviour {

    public const string SCENENAME = "InGameUI";

    public PlayerInput playerInputs;
    public SmashDisplay smashDisplay;
    public DialogDisplay dialogDisplay;
    public CanvasGroup gameRelatedGroup;
    public BossHealthBarDisplay bossHealthBar;
    public RectTransform stayWithinGameView;

    [Header("16:9")]
    public HealthDisplay healthdisplayVertical;
    public OptionsButton optionsButtonLow;

    [Header("4:3")]
    public HealthDisplay healthdisplayHorizontal;
    public OptionsButton optionsButtonHigh;

    public void Init(PlayerController playerController)
    {
        smashDisplay.Init(playerController);
        playerInputs.Init(playerController);

        float aspect = Game.instance.gameCamera.cam.aspect;

        //Choose hp display
        if (aspect > 1.486f)
        {
            //16:9
            healthdisplayVertical.Init();
            healthdisplayHorizontal.gameObject.SetActive(false);
        }
        else
        {
            //4:3
            healthdisplayHorizontal.Init();
            healthdisplayVertical.gameObject.SetActive(false);
        }

        //Choose options button
        if (aspect > 1.361f)
        {
            //16:9
            optionsButtonLow.gameObject.SetActive(true);
            optionsButtonHigh.gameObject.SetActive(false);
        }
        else
        {
            //4:3
            optionsButtonLow.gameObject.SetActive(false);
            optionsButtonHigh.gameObject.SetActive(true);
        }
    }
}
