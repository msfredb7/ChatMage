using CCC.Manager;
using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class TutorialScene : MonoBehaviour
    {
        public const string SCENENAME = "Tutorial";

        public ProxyButton proxyButton;
        public Spotlight spotlight;
        public TextDisplay textDisplay;
        public InputDisabler inputDisabler;
        public Shortcuts shorcuts;
        public DelayedAction delayedAction;
        public WaitForInput waitForInput;

        /// <summary>
        /// Retourne faux si le tutoriel a deja ete completer
        /// </summary>
        public static bool StartTutorial(string tutorialAssetName)
        {
            //Check if completed ?
            bool hasBeenCompleted = BaseTutorial.HasBeenCompleted(tutorialAssetName);

            if (hasBeenCompleted)
                return false;

            // Load la scene + lance le tuto
            ResourceLoader.LoadTutorialAsync(tutorialAssetName, delegate (BaseTutorial tutorial)
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    Scenes.FindRootObject<TutorialScene>(scene).Init(tutorial);
                });
            });

            return true;
        }

        /// <summary>
        /// Force le tutoriel a être exécuté
        /// </summary>
        public static void ForceStartTutorial(string tutorialAssetName)
        {
            // Load la scene + lance le tuto
            ResourceLoader.LoadTutorialAsync(tutorialAssetName, delegate (BaseTutorial tutorial)
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    Scenes.FindRootObject<TutorialScene>(scene).Init(tutorial);
                });
            });
        }

        public bool Init(BaseTutorial tutorial)
        {
            if (tutorial == null)
                return false;

            shorcuts = new Shortcuts(this);

            tutorial.Init(this);

            return true;
        }
    }

}