using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadout : MonoBehaviour
{
    public const string SCENENAME = "LoadoutMenu";
    public Armory armory;

    private string levelScriptName;

    public void Init(string levelScriptName)
    {
        this.levelScriptName = levelScriptName;
    }

    public void TestClick()
    {
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(armory.DebugGetCar().equipableAssetName, armory.DebugGetCar().type);
        loadoutResult.AddEquipable(armory.DebugGetSmash().equipableAssetName, armory.DebugGetSmash().type);
        loadoutResult.AddEquipable(armory.DebugGetItem().equipableAssetName, armory.DebugGetItem().type);

        LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, loadoutResult), false);
    }
}
