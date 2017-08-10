using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FullInspector;

namespace GameEvents
{
    [ExecuteInEditMode]
    public class FIPhysicalEvent : BaseBehavior, INodedEvent
    {
        [HideInInspector]
        public Rect windowRect = new Rect(300, 300, 150, 10);
        [HideInInspector]
        public EventGraph graph;

        public EventGraph Graph
        {
            get
            {
                return graph;
            }
            set
            {
                graph = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            LinkToGraph();
        }

        public bool LinkToGraph()
        {
            if (!Application.isPlaying)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.isLoaded)
                {
                    graph = scene.FindRootObject<EventGraph>();
                    if (graph != null)
                    {
                        if (!graph.ContainsEvent(this))
                            graph.AddEvent(this);
                        return true;
                    }
                    else
                    {
                        Debug.LogError("No EventGraph component on root objects. Removing PhysicalEvent component.");
                        DestroyImmediate(this);
                    }
                }
            }
            return false;
        }

        protected virtual void OnDestroy()
        {
            if (!Application.isPlaying && graph != null)
                graph.RemoveEvent(this);
        }

        public Rect WindowRect
        {
            get
            {
                return windowRect;
            }
            set
            {
                windowRect = value;
            }
        }

        public void ResetWindowRectPos()
        {
            windowRect = new Rect(250, 250, windowRect.width, windowRect.height);
        }
        public void ResetWindowRectSize()
        {
            windowRect = new Rect(windowRect.x, windowRect.y, 150, 10);
        }

        public virtual Color DefaultColor() { return Color.white; }

        public UnityEngine.Object AsObject() { return this; }

        public virtual void Trigger()
        {

        }

        public bool CanBeManuallyDestroyed() { return false; }

        public virtual string DefaultLabel() { return "Base Physical"; }

        public string TypeLabel() { return "Physical"; }

        public virtual void GetAdditionalMoments(out Moment[] moments, out string[] names)
        {
            moments = null;
            names = null;
        }

        public bool AcceptExternalTriggering()
        {
            return true;
        }

        public void MoveToPos(Vector2 position)
        {
            windowRect.position = position;
        }
    }
}
