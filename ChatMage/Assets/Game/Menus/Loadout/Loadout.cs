using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadout : MonoBehaviour
{
    public Armory armory;

    private LevelScript currentLevel;

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
    }

    public void TestClick()
    {
        LoadoutResult loadoutResult = new LoadoutResult();
        loadoutResult.AddEquipable(armory.DebugGetCar().equipableAssetName, armory.DebugGetCar().type);
        loadoutResult.AddEquipable(armory.DebugGetSmash().equipableAssetName, armory.DebugGetSmash().type);
        loadoutResult.AddEquipable(armory.DebugGetItem().equipableAssetName, armory.DebugGetItem().type);
        LoadingScreen.TransitionTo("Framework", new LoadoutMessage(currentLevel, loadoutResult), false);
    }
}
