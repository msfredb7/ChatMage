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

    void Awake()
    {
        MasterManager.Sync();
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
        if (!IsRegionAccessible(world.regions[currentRegion.regionNumber + 1]))
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
                        LoadingScreen.TransitionTo("LoadoutMenu", new ToLoadoutMessage(region.levels[localI].levelScript), false);
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

    bool IsRegionAccessible(Region region)
    {
        bool result = false;
        for (int i = 0; i < region.levels.Count; i++)
        {
            if (region.levels[i].unlock == true)
                result = true;
        }
        return result;
    }

    public void UpdateWorld(LevelScript level, bool result)
    {
        if (!result)
            return;

        Level completedLevel = world.GetLevelByLevelScript(level);

        for (int i = 0; i < completedLevel.nextLevels.Count; i++)
        {
            completedLevel.nextLevels[i].unlock = true;
        }
    }

    public void LoadScene(string name)
    {
        LoadingScreen.TransitionTo(name, null);
    }
}
