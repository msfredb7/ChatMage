using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEvents
{
    public class VirtualEvent : ScriptableObject, INodedEvent
    {
        [HideInInspector]
        public Rect windowRect = new Rect(200, 200, 150, 10);
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

        public void ResetWindowRectPos()
        {
            windowRect = new Rect(250, 250, windowRect.width, windowRect.height);
        }
        public void ResetWindowRectSize()
        {
            windowRect = new Rect(windowRect.x, windowRect.y, 150, 10);
        }

        public void MoveToPos(Vector2 position)
        {
            windowRect.position = position;
        }

        public virtual Color GUIColor() { return Color.white; }

        public UnityEngine.Object AsObject() { return this; }

        public bool CanBeManuallyDestroyed() { return true; }

        public virtual string NodeLabel() { return "Base Virtual"; }

        public string TypeLabel() { return "Virtual"; }

        public virtual void GetAdditionalMoments(out BaseMoment[] moments, out string[] names)
        {
            moments = null;
            names = null;
        }
    }
}