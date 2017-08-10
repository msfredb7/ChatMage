using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class BasePhysicalEvent : MonoBehaviour, IEventDisplay, IEvent
{
    [HideInInspector]
    public Rect windowRect = new Rect(300, 300, 150, 10);
    [HideInInspector]
    public EventGraph graph;

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.isLoaded)
            {
                graph = scene.FindRootObject<EventGraph>();
                if (graph != null)
                {
                    if (!graph.ContainsPhysicalEvent(this))
                        graph.AddPhysicalEvent(this);
                }
                else
                {
                    Debug.LogError("No EventGraph component on root objects. Removing PhysicalEvent component.");
                    DestroyImmediate(this);
                }
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (!Application.isPlaying && graph != null)
            graph.RemovePhysicalEvent(this);
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
}
