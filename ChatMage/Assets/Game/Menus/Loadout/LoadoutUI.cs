using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;
using System.Collections.ObjectModel;

namespace LoadoutMenu
{
    public enum LoadoutTab { Car = 0, Smash = 1, Items = 2, Recap = 3 }

    public class LoadoutUI : BaseBehavior
    {
        public const string SCENENAME = "LoadoutMenu";

        [InspectorHeader("Linking")]
        public Armory armory;
        public LoadoutElementInspector inspector;
        public LoadoutSideButtons backButton;
        public LoadoutSideButtons nextButton;
        public LoadoutGrid grid;
        public LoadoutPanelAnimator panelAnimator;
        public LoadoutProgressPanel progressPanel;
        public LoadoutTextChanger[] textChangers;

        [InspectorMargin(10), InspectorHeader("Debug - A enlever")]
        public Level debugLevel;
        public LoadoutTab debugStartTab = LoadoutTab.Car;

        [InspectorMargin(10), InspectorHeader("Live Data"), InspectorDisabled()]
        public Loadout currentLoadout;

        private LoadoutTab currentTab;
        private string levelScriptName;
        private LoadoutElement previouslySelectedElement;

        [InspectorButton]
        private void DebugLaunch()
        {
            MasterManager.Sync(delegate ()
            {
                Init(debugLevel.levelScriptName, debugStartTab);
            });
        }

        protected override void Awake()
        {
            base.Awake();

            //Setup listeners
            grid.onElementClicked += OnGrid_ElementClicked;
            nextButton.onClick += OnSideButton_NextClick;
            backButton.onClick += OnSideButton_BackClick;
            inspector.onBuySlotsClicked += OnInspector_BuySlotClick;
            inspector.onEquipClicked += OnInspector_EquipClick;
            inspector.onUnequipClicked += OnInspector_UnequipClick;
            inspector.onShopClicked += OnInspector_ShopClick;

            //On cache l'inspecteur par defaut
            inspector.Fill(null);
        }

        public void Init(string levelScriptName, LoadoutTab startTab = LoadoutTab.Car)
        {
            this.levelScriptName = levelScriptName;
            armory.Load();

            LoadoutResult lastLoadoutResult = LoadoutResult.Load();

            //TODO Changer les List<T> dans armory pour des arrays
            currentLoadout = new Loadout(armory.cars.ToArray(), armory.smashes.ToArray(), armory.items.ToArray(), armory.ItemSlots, lastLoadoutResult);

            //Top panel qui suis la progression dans le loadout
            progressPanel.Init(currentLoadout);

            // Back Button
            backButton.Show(true);

            // Next Nutton
            nextButton.Show(true);

            // Load First Tab
            currentTab = startTab;
            progressPanel.SetTab(currentTab, false);
            FillUI();
            VerifiyNextButton();
        }

        void FillUI()
        {
            //Cache le previewer
            inspector.Fill(null);

            switch (currentTab)
            {
                case LoadoutTab.Car:
                    grid.Fill(currentLoadout.cars);
                    break;
                case LoadoutTab.Smash:
                    grid.Fill(currentLoadout.smashes);
                    break;
                case LoadoutTab.Items:
                    grid.Fill(currentLoadout.items);
                    break;
                case LoadoutTab.Recap:
                    Debug.LogWarning("Pas encore impl�ment�.");
                    break;
                default:
                    throw new System.Exception("hmm, le jeu est bris�!");
            }

            //Ceci est pour set divers chose (ex: titre de la tab, bouton next, etc.)
            for (int i = 0; i < textChangers.Length; i++)
            {
                textChangers[i].SetCategory(currentTab);
            }
        }

        #region Events des sideButtons (Back/Next)

        private void OnSideButton_BackClick()
        {
            backButton.Interactable = false;
            nextButton.Interactable = false;

            //On est au bout ?
            if (currentTab == 0)            //�ventuellement chang� �a pour 3 (loadout recap)
            {
                BackToLevelSelect();
            }
            else
            {
                currentTab--;

                //Update le progress panel
                progressPanel.SetTab(currentTab, true);

                //Exit
                panelAnimator.ExitRight(delegate ()
                {
                    FillUI();

                    //Enter
                    panelAnimator.EnterLeft(delegate ()
                    {
                        //Animation done !
                        backButton.Interactable = true;
                        VerifiyNextButton();
                    });
                });
            }
        }

        private void OnSideButton_NextClick()
        {
            backButton.Interactable = false;
            nextButton.Interactable = false;


            //On est a la derniere tab ?
            if ((int)currentTab == 2)            //�ventuellement chang� �a pour 3 (loadout recap)
            {
                LauchGame();
            }
            else
            {
                currentTab++;

                //Update le progress panel
                progressPanel.SetTab(currentTab, true);

                //Exit
                panelAnimator.ExitLeft(delegate ()
                {
                    FillUI();

                    //Enter
                    panelAnimator.EnterRight(delegate ()
                    {
                        //Animation done !
                        backButton.Interactable = true;
                        VerifiyNextButton();

                    });
                });
            }
        }
        #endregion

        #region Events de la grille

        //Quand on click sur un element, on l'affiche dans l'inspecteur
        private void OnGrid_ElementClicked(LoadoutElement element)
        {
            if (previouslySelectedElement != null)
                previouslySelectedElement.VisuallySelected = false;

            inspector.Fill(element != null ? element.Equipable : null);

            //If all this is an item and all item slots are full, show 'buy slots' button
            if (element != null &&
                element.Equipable.preview.type == EquipableType.Item &&
                !element.Equipable.IsEquipped &&
                !currentLoadout.CanEquipMoreItems)
            {
                inspector.ShowBuySlotsButton();
            }

            previouslySelectedElement = element;
        }

        #endregion

        #region Events de l'inspecteur

        private void OnInspector_EquipClick(LoadoutEquipable equipable)
        {
            bool success = currentLoadout.Equip(equipable);

            if (success)
            {
                //Vide l'inspecteur.
                //Autre option: On pourrait simplement update l'inspecteur
                //              Crainte: Le joueur double tap accidentellement sur le boutton equip/unequip
                //                       qui sont exactement a la meme position

                //On simule la selection d'un element null.
                OnGrid_ElementClicked(null);
                VerifiyNextButton();
            }
            else
            {
                throw new System.Exception("Failed to equip the following equipable: " + equipable.preview.name);
            }
        }
        private void OnInspector_UnequipClick(LoadoutEquipable equipable)
        {
            currentLoadout.UnEquip(equipable);

            //Vide l'inspecteur.
            //Autre option: On pourrait simplement update l'inspecteur
            //              Crainte: Le joueur double tap accidentellement sur le boutton equip/unequip
            //                       qui sont exactement a la meme position

            //On simule la selection d'un element null.
            OnGrid_ElementClicked(null);
            VerifiyNextButton();
        }
        private void OnInspector_ShopClick(LoadoutEquipable equipable)
        {
            GoToShop();
        }
        private void OnInspector_BuySlotClick(LoadoutEquipable equipable)
        {
            BuySlots();
        }

        private void VerifiyNextButton()
        {
            switch (currentTab)
            {
                case LoadoutTab.Car:
                    nextButton.Interactable = currentLoadout.chosenCar != null;
                    break;
                case LoadoutTab.Smash:
                    nextButton.Interactable = currentLoadout.chosenSmash != null || currentLoadout.smashes.Length <= 0;
                    break;
                case LoadoutTab.Items:
                    nextButton.Interactable = true;
                    break;
                case LoadoutTab.Recap:
                    nextButton.Interactable = true;
                    break;
            }
        }

        #endregion

        #region Other scenes

        public void BuySlots()
        {
            //TODO: Pour etre plus clean, on devrait passer par un proxy de shop ou d'account

            Debug.LogWarning("Pas implémenté");

            //PopUpMenu.ShowOKPopUpMenu("Are you sure you want to buy an extra slots for items ?", delegate ()
            //{
            //    if ((Account.instance.GetMoney() - 10) < 0)
            //        PopUpMenu.ShowPopUpMenu("You don't have enough money. Open loot boxes or win levels to gain money. See you later!", 2);
            //    else
            //        armory.BuyItemSlots(1, -10);
            //});
        }

        public void GoToShop()
        {
            SaveLoadout();
            LoadingScreen.TransitionTo(ShopMenu.SCENENAME, new ToShopMessage(SCENENAME, currentTab));
        }

        public void LauchGame()
        {
            LoadoutResult result = SaveLoadout();
            LoadingScreen.TransitionTo(Framework.SCENENAME, new ToGameMessage(levelScriptName, result), true);
        }

        public void BackToLevelSelect()
        {
            SaveLoadout();
            LoadingScreen.TransitionTo(LevelSelection.SCENENAME, null);
        }

        public LoadoutResult SaveLoadout()
        {
            LoadoutResult result = currentLoadout.ProduceResult();
            result.Save();

            return result;
        }
        #endregion
    }
}
