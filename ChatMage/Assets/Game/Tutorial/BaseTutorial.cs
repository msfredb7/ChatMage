using CCC.Manager;
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
            [InspectorHeader("Use a milestone to start the event")]
            public bool useMileStone = false;
            [InspectorShowIf("useMileStone")]
            public List<string> milestoneThatTrigger = new List<string>();
            [InspectorTooltip("If at false, time counter won't start from" +
                "beginning but from where it was trigger (example : from outside)")]
            public bool invokeOnGameStarted = true;
            [InspectorHeader("Will start this event when the other one is over")]
            public bool startAfterAnEvent = false;
            [InspectorShowIf("startAfterAnEvent")]
            public int tutorialEventIndex;
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

        public List<TutorialEvent> tutorialEvents = new List<TutorialEvent>();
        public bool startOnInit = true;

        [NonSerialized, fsIgnore]
        protected TutorialScene modules;

        private const string TUTORIALSAVE = "cmplt"; //Short pour 'Completed'

        public virtual void Init(TutorialScene modules)
        {
            this.modules = modules;

            if (startOnInit)
                Start();
        }

        /// <summary>
        /// Debut du tutoriel
        /// </summary>
        public void Start()
        {
            // On s'assure que les creation de sauvegarde dans begin on bien sauvegarder
            //GameSaves.instance.SaveData(GameSaves.Type.Tutorial);

            // Avant meme de commencer a faire les events, on doit s'assurer que l'enchainement se fera comme il faut
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                TutorialEvent currentTutorial = tutorialEvents[i];
                if (currentTutorial.startAfterAnEvent)
                {
                    tutorialEvents[currentTutorial.tutorialEventIndex].onComplete += delegate () { Execute(currentTutorial, true); };
                    // Never Execute It Again
                    currentTutorial.alreadyExecute = true;
                }

            }

            // On peut ensuite commencer les events qui sont start au debut
            for (int i = 0; i < tutorialEvents.Count; i++)
            {
                if (tutorialEvents[i].alreadyExecute)
                    continue;
                if (tutorialEvents[i].invokeOnGameStarted || tutorialEvents[i].useSpecificTime)
                    Execute(tutorialEvents[i], true);
            }

            OnStart();
        }

        protected abstract void OnStart();

        /// <summary>
        /// Fin de la partie
        /// </summary>
        public void End(bool markAsCompleted = true)
        {
            if (markAsCompleted)
            {
                GameSaves.instance.SetBool(GameSaves.Type.Tutorial, TUTORIALSAVE + name, true);
                GameSaves.instance.SaveDataAsync(GameSaves.Type.Tutorial, Quit);
            }
            else
            {
                Quit();
            }
        }

        protected abstract void Cleanup();

        private void Quit()
        {
            Cleanup();
            modules = null;
            Scenes.UnloadAsync(TutorialScene.SCENENAME);
        }

        public static bool HasBeenCompleted(string assetName)
        {
            if (!GameSaves.instance.ContainsBool(GameSaves.Type.Tutorial, TUTORIALSAVE + assetName))
                GameSaves.instance.SetBool(GameSaves.Type.Tutorial, TUTORIALSAVE + assetName, false);
            return GameSaves.instance.GetBool(GameSaves.Type.Tutorial, TUTORIALSAVE + assetName);
        }

        public void Execute(TutorialEvent tutorialEvent, bool useTime)
        {
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