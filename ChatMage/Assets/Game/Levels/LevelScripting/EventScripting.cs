using DG.Tweening;
using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LevelScripting
{
    [System.Serializable]
    public class EventScripting
    {
        public class EventScriptingWhat
        {
            [InspectorTooltip("Who's the script with the event")]
            public string scriptName;
            [InspectorTooltip("Function name to cast the event")]
            public string functionName;
            public bool startNextEvent;
        }
        public class EventScriptingWhen
        {
            public bool useSpecificTime = false;
            [InspectorHideIf("useSpecificTime")]
            public bool invokeOnStart = true;
            [InspectorHideIf("useSpecificTime")]
            public float when;
        }

        [InspectorCategory("What")]
        public EventScriptingWhat eventWhat;
        [InspectorCategory("When")]
        public EventScriptingWhen eventWhen;

        [HideInInspector]
        public delegate void inGameEvents(EventScripting ev);
        [HideInInspector]
        public event inGameEvents onComplete;
        [HideInInspector]
        public bool done;

        public void Init()
        {
            // Si on a fait un event custom
            if(eventWhen != null)
            {
                if (eventWhen.invokeOnStart)
                {
                    Sequence sq = DOTween.Sequence();
                    sq.InsertCallback(eventWhen.when, delegate () { Launch(); });
                }
            }
            done = false;
        }

        /// <summary>
        /// Debute l'evennement selon le nom de la fonction. Attention, pas de paramettres !
        /// </summary>
        public void Launch()
        {
            Type thisType = Type.GetType(eventWhat.scriptName);
            MethodInfo theMethod = thisType.GetMethod(eventWhat.functionName);
            theMethod.Invoke(Game.instance.currentLevel, null);
            onComplete.Invoke(this);
            done = true;
        }
    }
}

