using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListDisplay : MonoBehaviour {

    public StageButton buttonPrefab;
    public Transform countainer;

    public InfiniteVerticalScroll infiniteScroller;

    private int stageAmountUnlocked;

	void Start ()
    {
        LoadInfo();
        SpawnButtons();
	}
	
	void LoadInfo()
    {
        stageAmountUnlocked = 10;
    }

    void SpawnButtons()
    {
        for (int i = 0; i < stageAmountUnlocked; i++)
        {
            StageButton newButton = Instantiate(buttonPrefab, countainer);
            newButton.SetButtonInfo(i);
        }
    }
}
