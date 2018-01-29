
using DG.Tweening;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using FullSerializer;

namespace Tutorial
{
    public abstract class BaseTutorial : BaseScriptableObject
    {
        public class TutorialEvent
        {
            //[InspectorTooltip("Who's the script with the event")]
            //public string scriptName;
            [InspectorTooltip("Function name to cast the event")]
            public string functionName;
            [InspectorHeader("Use a specific time to start the event")]
            public bool useSpecificTime = false;
            [InspectorShowIf("useSpecificTime")]
            public float when = 0;
            [InspectorTooltip("If at false, time counter won't start from" +
    "beginning but from where it was trigger (example : from outside)")]
            public bool invokeOnGameStarted = true;
            [InspectorHeader("Use a milestone to start the event"),InspectorHideIf("invokeOnGameStarted")]
            public bool useMileStone = false;
            [InspectorHideIf("invokeOnGameStarted")]
            public List<string> milestoneThatTrigger = new List<string>();
            [InspectorHeader("Use a Unit Wave to start the event"), InspectorHideIf("invokeOnGameStarted")]
            public bool useUnitWave = false;
            [InspectorHideIf("invokeOnGameStarted")]
            public bool onUnitWaveLaunch = false;
            [InspectorHideIf("invokeOnGameStarted")]
            public List<string> unitWaveThatTrigger = new List<string>();
            [InspectorHeader("Will start this event when the other one is over"), InspectorHideIf("invokeOnGameStarted")]
            public bool startAfterAnEvent = false;
            [InspectorShowIf("startAfterAnEvent")]
            public int startAfterTutorialEventIndex;
            public SimpleEvent onComplete;

            [HideInInspector, NonSerialized]
            public bool alreadyExecute = false;

            public void OnComplete()
            {
                if (onComplete != null)
                {
                    onComplete.Invoke();
                }
                Debug.Log(functionName + "Event Complete");
            }
        }
        public DataSaver dataSaver;
        public bool startTutorialOnInit = true;
        public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();

        [NonSerialized, fsIgnore]
        protected TutorialScene modules;

        private const string TUTORIALSAVE = "cmplt"; //Short pour 'Completed'

        protected LevelScript currentLevel;

        public virtual void Init(TutorialScene modules)
        {
            this.modules = modules;

            if(Game.Instance != null)
                currentLevel = Game.Instance.levelScript;

            if (startTutorialOnInit)
                Start();
        }

        /// <summary>
        /// Debut du tutoriel
        /// </summary>
        public void Start()
        {
            // On s'assure que les creation de sauvegarde dans begin on bien sauvegarder
            //GameSaves.instance.SaveData(GameSaves.Type.Tutorial);

            // �couter aux milestones
            if(currentLevel != null)
                currentLevel.onEventReceived += CurrentLevel_onEventReceived;

            // Avant meme de commencer a faire les events, on doit s'assurer que l'enchainement se fera comme il faut
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                TutorialEvent currentTutorialEvent = tutorialEvents[i];
                if (currentTutorialEvent.startAfterAnEvent)
                {
                    tutorialEvents[currentTutorialEvent.startAfterTutorialEventIndex].onComplete += delegate () { Execute(currentTutorialEvent, true); };
                    // Never Execute It Again
                    currentTutorialEvent.alreadyExecute = true;
                }

            }

            // On peut ensuite commencer les events qui sont start au debut
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                TutorialEvent currentEvent = tutorialEvents[i];
                if (currentEvent.alreadyExecute)
                    continue;
                if (currentEvent.invokeOnGameStarted)
                    Execute(currentEvent, true);
                else if(currentEvent.useUnitWave && currentEvent.onUnitWaveLaunch)
                {
                    foreach (string unitWave in currentEvent.unitWaveThatTrigger)
                    {
                        currentLevel.AddEventOnLaunchedUnitWave(unitWave, delegate ()
                        {
                            Execute(currentEvent, true);
                        });
                    }
                }
            }

            OnStart();
        }

        private void CurrentLevel_onEventReceived(string message)
        {
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                if (tutorialEvents[i].useMileStone)
                {
                    for (int j = 0; j < tutorialEvents[i].milestoneThatTrigger.Count; j++)
                    {
                        if (tutorialEvents[i].milestoneThatTrigger[j] == message)
                        {
                            Execute(tutorialEvents[i], true);
                        }
                    }
                } else if (tutorialEvents[i].useUnitWave && !tutorialEvents[i].onUnitWaveLaunch)
                {
                    for (int j = 0; j < tutorialEvents[i].unitWaveThatTrigger.Count; j++)
                    {
                        if (tutorialEvents[i].unitWaveThatTrigger[j] == message)
                        {
                            Execute(tutorialEvents[i], true);
                        }
                    }
                }
            }
        }

        protected abstract void OnStart();

        /// <summary>
        /// Fin du tutoriel
        /// </summary>
        public void End(bool markAsCompleted)
        {
            if (markAsCompleted)
            {
                dataSaver.SetBool(TUTORIALSAVE + name, true);
                dataSaver.SaveAsync(Quit);
            }
            else
            {
                Quit();
            }
        }
        public void End()
        {
            End(true);
        }

        protected virtual void Cleanup()
        {
            if(currentLevel != null)
                currentLevel.onEventReceived -= CurrentLevel_onEventReceived;
        }

        private void Quit()
        {
            Cleanup();
            modules = null;
            Scenes.UnloadAsync(TutorialScene.SCENENAME);
        }

        public static bool HasBeenCompleted(string assetName, DataSaver tutorialSaver)
        {
            if (!tutorialSaver.ContainsBool(TUTORIALSAVE + assetName))
                tutorialSaver.SetBool(TUTORIALSAVE + assetName, false);
            return tutorialSaver.GetBool(TUTORIALSAVE + assetName);
        }

        public void Execute(TutorialEvent tutorialEvent, bool useTime)
        {
            if (tutorialEvent.alreadyExecute)
                return;
            tutorialEvent.alreadyExecute = true;

            // Find the Action/Method to Invoke
            Type thisType = GetType();
            MethodInfo theMethod = thisType.GetMethod(tutorialEvent.functionName);

            // Gere le temps
            Sequence sequence = DOTween.Sequence().SetUpdate(true);

            // Parametre de la method = Fonction On Complete
            Action[] parameters;
            parameters = new Action[1];
            parameters[0] = delegate ()
            {
                tutorialEvent.OnComplete();
            };

            if (useTime && tutorialEvent.useSpecificTime)
                sequence.InsertCallback(tutorialEvent.when, delegate ()
                {
                    theMethod.Invoke(this, parameters);
                });
            else
            {
                theMethod.Invoke(this, parameters);
            }
        }
    }
}