using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelect
{
    public class LevelSelection : MonoBehaviour
    {
        public const string SCENENAME = "LevelSelect";

        private const string LASTLEVELSELECTED_KEY = "lls";

        [Header("Linking")]
        public List<LevelSelect_Region> regions;
        public Button backButton;
        public Button shopButton;
        public LevelSelect_MapAnimator mapAnimator;

        public Button demoButton;
        public Level demoLevel;
        public bool DEMO = false; // A ENLEVER

        void Start()
        {
            MasterManager.Sync(OnSync);
            backButton.onClick.AddListener(OnBackClicked);
            shopButton.onClick.AddListener(OnShopClicked);
        }

        void OnSync()
        {
            if (DEMO)
            {
                demoButton.gameObject.SetActive(true);
                ToLoadoutMessage message = new ToLoadoutMessage(demoLevel.levelScriptName);
                demoButton.onClick.AddListener(delegate() { LoadingScreen.TransitionTo(LoadoutMenu.LoadoutUI.SCENENAME, message); });
                for (int i = 0; i < regions.Count; i++)
                {
                    regions[i].HideAll();
                }
                return;
            }

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

            mapAnimator.SetLastUnlockedRegionIndex(GetLastUnlockedRegion());

            //NOTE: Quand on va vouloir implémenté des animation forcés (ex: unlock un nouveau niveau / une nouvelle région)
            //      on va devoir mettre une variable sauvegardé dans Level du style: 'bool hasBeenSeen'
            //      si le niveau est unlocked MAIS que hasBeenSeen est faux, on met hasBeenSeen à vrai (+ on sauvegarde)
            //      on lance ensuite l'animation !
            //      Les fonctions pour déplacer la map de manière 'animé' sont déjà présentes et fonctionnelles 
            //      dans LevelSelect_MapAnimator
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

        private int GetLastUnlockedRegion()
        {
            for (int i = 0; i < regions.Count; i++)
            {
                if (!regions[i].IsUnlocked())
                    return i--;
            }
            return regions.Count - 1;
        }
    }
}