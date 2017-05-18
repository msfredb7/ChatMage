using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadout : MonoBehaviour
{
    [Header("TEMPORAIRE")]
    public LevelScript chosenLevel;

    [Header("Incomplet")]
    public EquipablePreview chosenCar;
    public EquipablePreview chosenSmash;
    public List<EquipablePreview> chosenItems;

    public void LaunchGame()
    {
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(chosenCar.equipableAssetName, chosenCar.type);
        loadoutResult.AddEquipable(chosenSmash.equipableAssetName, chosenSmash.type);
        for (int i = 0; i < chosenItems.Count; i++)
        {
            loadoutResult.AddEquipable(chosenItems[i].equipableAssetName, chosenItems[i].type);
        }
        LaunchGameMessage message = new LaunchGameMessage(chosenLevel, loadoutResult);

        LoadingScreen.TransitionTo(Framework.SCENENAME, message);
    }
}
