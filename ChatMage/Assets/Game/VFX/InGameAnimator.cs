using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class InGameAnimator : MonoBehaviour
{
    public Animator controller;

    protected virtual void UpdateTimescale(float worldTimescale)
    {
        controller.speed = worldTimescale;
    }

    protected void AddTimescaleListener()
    {
        if(Game.instance == null)
        {
            Debug.LogError("Could not add listener to worldTimescale. Game.instance == null");
            return;
        }

        StatFloat worldTimescale = Game.instance.worldTimeScale;
        worldTimescale.onSet.AddListener(UpdateTimescale);
        UpdateTimescale(worldTimescale);
    }

    protected void RemoveTimescaleListener()
    {
        StatFloat worldTimescale = Game.instance.worldTimeScale;
        worldTimescale.onSet.RemoveListener(UpdateTimescale);
    }
}
