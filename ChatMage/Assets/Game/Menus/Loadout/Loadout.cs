using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FullInspector;

public class Loadout : BaseBehavior
{
    public Armory armory;

    private string levelScriptName;

    public GameObject button;

    public GameObject itemButtonsCountainer;
    public GameObject smashButtonsCountainer;
    public GameObject carButtonsCountainer;

    [InspectorDisabled()]
    public LoadoutResult currentLoadout;

    public const string SCENENAME = "LoadoutMenu";

    public void Init(string levelScriptName)
    {
        this.levelScriptName = levelScriptName;
        armory.Load();
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

    private void OnDestroy()
    {
        // Sauvegarde du Loadout !
    }

    public void ChargeLoadoutAndGame()
    {
        LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, currentLoadout), false);
    }
}
