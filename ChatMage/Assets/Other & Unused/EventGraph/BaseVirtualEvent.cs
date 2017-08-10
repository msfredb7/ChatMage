using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVirtualEvent : ScriptableObject, IEventDisplay, IEvent
{
    [HideInInspector]
    public Rect windowRect = new Rect(200, 200, 150, 10);

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

    public void MoveToPos(Vector2 position)
    {
        windowRect.position = position;
    }

    public virtual Color DefaultColor() { return Color.white; }

    public UnityEngine.Object AsObject() { return this; }

    public virtual void Trigger()
    {

    }

    public bool CanBeManuallyDestroyed() { return true; }

    public virtual string DefaultLabel() { return "Base Virtual"; }

    public string TypeLabel() { return "Virtual"; }

    public virtual void GetAdditionalMoments(out Moment[] moments, out string[] names)
    {
        moments = null;
        names = null;
    }
}
