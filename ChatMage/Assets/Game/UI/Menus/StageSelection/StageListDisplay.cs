using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListDisplay : MonoBehaviour {

    public StageButton buttonPrefab;
    public Transform countainer;

    public InfiniteVerticalScroll infiniteScroller;

    public DataSaver datasaver;

    private int stageAmountUnlocked;

	void Start ()
    {
        LoadInfo();
        SpawnButtons();
	}
	
	void LoadInfo()
    {
        if (!datasaver.ContainsInt(LS_EndlessLevel.bestStepKey))
            datasaver.SetInt(LS_EndlessLevel.bestStepKey, 1);
        stageAmountUnlocked = Mathf.FloorToInt(datasaver.GetInt(LS_EndlessLevel.bestStepKey)/LS_EndlessLevel.stepToResetSave)+1;
    }

    void SpawnButtons()
    {
        for (int i = 0; i < stageAmountUnlocked; i++)
        {
            StageButton newButton = Instantiate(buttonPrefab, countainer);
            newButton.SetButtonInfo(i+1);
        }
    }
}
