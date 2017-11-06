using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameTimescaleListener : MonoBehaviour
{
    protected bool listenersOn = false;

    protected abstract void UpdateTimescale(float worldTimescale);

    protected void UpdateTimescale()
    {
        UpdateTimescale(Game.instance.worldTimeScale);
    }

    protected void AddTimescaleListener()
    {

        if (Game.instance == null)
        {
            Debug.LogError(name + " tried to a add listener to worldTimescale but Game.instance == null");
            return;
        }

        StatFloat worldTimescale = Game.instance.worldTimeScale;

        if (!listenersOn)
        {
            worldTimescale.onSet.AddListener(UpdateTimescale);
            listenersOn = true;
        }

        UpdateTimescale(worldTimescale);
    }

    protected void RemoveTimescaleListener()
    {

        if (Game.instance == null)
        {
            Debug.LogError(name + " tried to a remove listener to worldTimescale but Game.instance == null");
            return;
        }

        StatFloat worldTimescale = Game.instance.worldTimeScale;

        if (listenersOn)
        {
            worldTimescale.onSet.RemoveListener(UpdateTimescale);
            listenersOn = false;
        }
    }

    protected virtual void OnDestroy()
    {
        if (Game.instance != null)
            RemoveTimescaleListener();
    }
}
