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
        public GameObject inputBlocker;

        public AudioClip levelSelectMusic;

        void Start()
        {
            MasterManager.Sync(OnSync);
            backButton.onClick.AddListener(OnBackClicked);
            shopButton.onClick.AddListener(OnShopClicked);
            SoundManager.PlayMusic(levelSelectMusic);
        }

        void OnSync()
        {
            AddListeners();

            //Un peu lourd ? Peut-être qu'on pourrait faire ça AVANT que le loading screen disparaisse (comme Framework)
            LoadAllData();

            mapAnimator.SetLastUnlockedRegionIndex(GetLastUnlockedRegion());

            VerifyNewLevelAnimation();
            //NOTE: Quand on va vouloir implémenté des animation forcés (ex: unlock un nouveau niveau / une nouvelle région)
            //      on va devoir mettre une variable sauvegardé dans Level du style: 'bool hasBeenSeen'
            //      si le niveau est unlocked MAIS que hasBeenSeen est faux, on met hasBeenSeen à vrai (+ on sauvegarde)
            //      on lance ensuite l'animation !
            //      Les fonctions pour déplacer la map de manière 'animé' sont déjà présentes et fonctionnelles 
            //      dans LevelSelect_MapAnimator
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

            GameSaves.instance.SetString(GameSaves.Type.Levels, LASTLEVELSELECTED_KEY, level.name);

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

        private int GetLastUnlockedRegion()
        {
            for (int i = 0; i < regions.Count; i++)
            {
                if (!regions[i].IsUnlocked())
                    return i - 1;
            }
            return regions.Count - 1;
        }

        public bool VerifyNewLevelAnimation()
        {
            for (int i = 0; i < regions.Count; i++)
            {
                for (int j = 0; j < regions[i].levelItems.Count; j++)
                {
                    if (regions[i].levelItems[j].IsUnlocked() && !regions[i].levelItems[j].hasBeenSeen)
                    {
                        if (inputBlocker)
                            inputBlocker.SetActive(true);
                        regions[i].levelItems[j].GetComponent<RoadMapPoint>().StartRoad(delegate ()
                        {
                            regions[i].levelItems[j].MarkAsSeen();
                            if (!VerifyNewLevelAnimation() && inputBlocker != null)
                                inputBlocker.SetActive(false);
                        });
                        return true;
                    }
                }
            }
            return false;
        }
    }
}