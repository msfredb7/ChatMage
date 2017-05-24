using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

    public GameObject countainer;
    public GameObject button;

    public World world;
    private Region currentRegion;

    public Button regionChangeRight;
    public Button regionChangeLeft;
    public Text regionDisplay;

    [Header("TEMPORAIRE")]
    public EquipablePreview chosenCar;
    public EquipablePreview chosenSmash;
    public List<EquipablePreview> chosenItems;

    public class LevelMessage : SceneMessage
    {
        private LevelScript chosenLevel;

        public LevelMessage(LevelScript level)
        {
            chosenLevel = level;
        }

        public void OnLoaded(Scene scene)
        {
            Loadout loadOut = Scenes.FindRootObject<Loadout>(scene);
            loadOut.Init(chosenLevel);
        }

        public void OnOutroComplete()
        {

        }
    }

	// Use this for initialization
	void Start () {
        regionChangeRight.onClick.AddListener(GoRight);
        regionChangeLeft.onClick.AddListener(GoLeft);
        currentRegion = world.GetRegion(0);
        regionDisplay.text = currentRegion.displayName;
        SettupLevelPanel(currentRegion);
    }

    void GoRight()
    {
        if (currentRegion.regionNumber + 1 >= world.regions.Count)
            return;
        currentRegion = world.GetRegion(currentRegion.regionNumber + 1);
        regionDisplay.text = currentRegion.displayName;
        SettupLevelPanel(currentRegion);
    }

    void GoLeft()
    {
        if (currentRegion.regionNumber - 1 < 0)
            return;
        currentRegion = world.GetRegion(currentRegion.regionNumber - 1);
        regionDisplay.text = currentRegion.displayName;
        SettupLevelPanel(currentRegion);
    }

    void SettupLevelPanel(Region region)
    {
        Clear();
        for (int i = 0; i < region.levels.Count; i++)
        {
            if (region.levels[i].unlock)
            {
                GameObject newButton = Instantiate(button, countainer.transform);
                newButton.GetComponentInChildren<Text>().text = "Niveau " + (i + 1);
                int localI = i;
                newButton.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    if (region.levels[localI].levelScript != null)
                        LoadingScreen.TransitionTo("LoadoutMenu", new LevelMessage(region.levels[localI].levelScript), false);
                });
            }
        }
    }

    void Clear()
    {
        foreach (Transform child in countainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
