using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour {

    public Button button;
    public Text stageName;
    public Level endlessLevel;

    private int stageNumber;

    public void SetButtonInfo(int stageNumber)
    {
        this.stageNumber = stageNumber;
        stageName.text = "STAGE " + stageNumber;
        button.onClick.AddListener(GoToStage);
    }

    private void GoToStage()
    {
        //Loadout (TEMPORAIRE)
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable("Car_Vadrouille", EquipableType.Car);

        //Scene message
        ToGameMessage gameMessage = new ToGameMessage(endlessLevel.levelScriptName, loadoutResult, true);

        // Save stage choice
        PlayerPrefs.SetInt(LS_EndlessLevel.stageKey, stageNumber);

        DefaultAudioSources.StopMusicFaded(1);

        LoadingScreen.TransitionTo(Framework.SCENENAME, gameMessage, true);
    }
}
