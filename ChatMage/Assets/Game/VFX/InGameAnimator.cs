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
            Debug.LogError(name + " tried to a add listener to worldTimescale but Game.instance == null");
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
