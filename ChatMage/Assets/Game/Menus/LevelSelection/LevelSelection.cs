using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public const string SCENENAME = "LevelSelect";
    private const string LASTLEVELSELECTED_KEY = "lls";

    [Header("Linking")]
    public List<LevelSelect_Region> regions;
    public Button backButton;
    public Button shopButton;
    public LevelSelectAnimation backgroundAnimation;

    private static List<LevelSelect_Region> previousRegions;

    void Start()
    {
        MasterManager.Sync(OnSync);
        backButton.onClick.AddListener(OnBackClicked);
        shopButton.onClick.AddListener(OnShopClicked);
    }

    void OnSync()
    {
        AddListeners();

        // Should we mark a level as 'completed' ?
        bool lastGameResult = GetLastGameResult();


        if (lastGameResult)
        {
            if (GameSaves.instance.ContainsString(GameSaves.Type.LevelSelect, LASTLEVELSELECTED_KEY))
            {
                //Le joueur a gagner sa dernière game, et nous avons la 'clé' du dernier lvl choisie
                string lastLevelSelected = GameSaves.instance.GetString(GameSaves.Type.LevelSelect, LASTLEVELSELECTED_KEY);

                //On marque le level comme complété
                MarkAsCompleted(lastLevelSelected);

                //On consomme la Win
                GameSaves.instance.SetBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY, false);
            }
            else
            {
                //Le joueur a une supposé 'victoire', mais nous n'avons pas la clé du dernier lvl choisie. C'est donc une win invalide
                GameSaves.instance.SetBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY, false);
            }

            //Save
            GameSaves.instance.SaveData(GameSaves.Type.LevelSelect);
        }

        //Un peu lourd ? Peut-être qu'on pourrait faire ça AVANT que le loading screen disparaisse (comme Framework)
        LoadAllData();

        if (previousRegions == null)
            previousRegions = regions;

        // Regarde si une region est maintenant disponible
        for (int i = 0; i < regions.Count; i++)
        {
            if (regions[i].IsUnlocked() != previousRegions[i].IsUnlocked())
                OnRegionUnlocked(i); // Une region a changer
        }

        SetBackgroundAnimationLimit();

        // On enregistre le contexte pour la prochaine fois
        previousRegions = regions;
    }

    bool GetLastGameResult()
    {
        if (GameSaves.instance.ContainsBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY))
        {
            return GameSaves.instance.GetBool(GameSaves.Type.LevelSelect, LevelScript.WINRESULT_KEY);
        }
        return false;
    }

    void AddListeners()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            regions[i].onLevelSelected += OnLevelSelected;
        }
    }

    void LoadAllData()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            regions[i].LoadData();
        }
    }

    void OnLevelSelected(Level level)
    {
        //Go to loadout !
        print("Level selected: " + level.name);

        GameSaves.instance.SetString(GameSaves.Type.LevelSelect, LASTLEVELSELECTED_KEY, level.name);

        ToLoadoutMessage message = new ToLoadoutMessage(level.levelScriptName);
        LoadingScreen.TransitionTo(LoadoutMenu.LoadoutUI.SCENENAME, message);
    }

    public void OnBackClicked()
    {
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }

    public void OnShopClicked()
    {
        LoadingScreen.TransitionTo(ShopMenu.SCENENAME, new ToShopMessage(SCENENAME));
    }

    private void MarkAsCompleted(string levelName)
    {
        for (int i = 0; i < regions.Count; i++)
        {
            if (regions[i].MarkAsCompleted(levelName))
                return;
        }
    }

    void OnRegionUnlocked(int index)
    {
        SetBackgroundAnimationLimit();

        // On enleve les inputs du joueur

        // On fait l'animation (doit avoir un callback)

        // On revient ou on était avant l'animation

        // On remet les inputs du joueur
    }

    void SetBackgroundAnimationLimit()
    {
        int previousRegionNumber = 0;

        // Trouver la dernier region lock
        for (int i = 0; i < regions.Count; i++)
        {
            if (!regions[i].IsUnlocked())
            {
                // On prend la region precedente qui etait unlock
                backgroundAnimation.SetLimitIndex(previousRegionNumber);
                return;
            }
            previousRegionNumber = i;
        }
        backgroundAnimation.SetLimitIndex(regions.Count - 1);
    }
}
