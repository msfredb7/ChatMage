using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    public Armory armory;

    private LevelScript currentLevel;

    public GameObject button;

    public GameObject itemButtonsCountainer;
    public GameObject smashButtonsCountainer;
    public GameObject carButtonsCountainer;

    public LoadoutResult currentLoadout;

    public class LoadoutMessage : SceneMessage
    {
        private LevelScript chosenLevel;
        private LoadoutResult loadoutResult;

        public LoadoutMessage(LevelScript level, LoadoutResult loadoutResult)
        {
            chosenLevel = level;
            this.loadoutResult = loadoutResult;
        }

        public void OnLoaded(Scene scene)
        {
            Framework framework = Scenes.FindRootObject<Framework>(scene);
            framework.Init(chosenLevel, loadoutResult);
        }

        public void OnOutroComplete()
        {

        }
    }

    public void Init(LevelScript level)
    {
        currentLevel = level;
        armory.Load();
        armory.DebugSetItemSlot(5);
        currentLoadout = new LoadoutResult(armory.GetItemSlots());

        List<EquipablePreview> unlockItems = armory.GetAllUnlockedItems();
        List<EquipablePreview> unlockCars = armory.GetAllUnlockedCars();
        List<EquipablePreview> unlockSmashes = armory.GetAllUnlockedSmash();

        for (int i = 0; i < unlockItems.Count; i++)
        {
            EquipablePreview currentEquipable = unlockItems[i];
            GameObject newButton = Instantiate(button, itemButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate() {
                currentLoadout.AddEquipable(currentEquipable.equipableAssetName, currentEquipable.type);
                Debug.Log(currentEquipable.displayName + " added");
            });
        }

        for (int i = 0; i < unlockCars.Count; i++)
        {
            EquipablePreview currentEquipable = unlockCars[i];
            GameObject newButton = Instantiate(button, carButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate () {
                currentLoadout.AddEquipable(currentEquipable.equipableAssetName, currentEquipable.type);
                Debug.Log(currentEquipable.displayName + " selected");
            });
        }

        for (int i = 0; i < unlockSmashes.Count; i++)
        {
            EquipablePreview currentEquipable = unlockSmashes[i];
            GameObject newButton = Instantiate(button, smashButtonsCountainer.transform);
            newButton.GetComponent<LoadoutButton>().ChangeLoadoutButton(currentEquipable.displayName);
            newButton.GetComponent<Button>().onClick.AddListener(delegate () {
                currentLoadout.AddEquipable(currentEquipable.equipableAssetName, currentEquipable.type);
                Debug.Log(currentEquipable.displayName + " selected");
            });
        }
    }

    public void ChargeLoadoutAndGame()
    {
        LoadingScreen.TransitionTo("Framework", new LoadoutMessage(currentLevel, currentLoadout), false);
    }
}
