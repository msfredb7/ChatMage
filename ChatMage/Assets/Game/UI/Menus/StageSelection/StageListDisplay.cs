using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListDisplay : MonoBehaviour {

    public StageButton buttonPrefab;
    public Transform countainer;

    public DataSaver datasaver;

    public int minimumStageUnlocked = 1;

    private int stageAmountUnlocked;

	void Start ()
    {
        LoadInfo();
        SpawnButtons();
	}
	
	void LoadInfo()
    {
        stageAmountUnlocked = Mathf.FloorToInt(datasaver.GetInt(LS_EndlessLevel.bestStepKey)/LS_EndlessLevel.stepToResetSave)+1;
        if (stageAmountUnlocked < minimumStageUnlocked)
            stageAmountUnlocked = minimumStageUnlocked;
        //Debug.Log(datasaver.GetInt(LS_EndlessLevel.bestStepKey));
    }

    void SpawnButtons()
    {
        for (int i = 1; i <= stageAmountUnlocked; i++)
        {
            StageButton newButton = Instantiate(buttonPrefab, countainer);
            newButton.SetButtonInfo(i);
        }
    }
}
