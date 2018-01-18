using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFlashAnimation
{
    private const float FLASH_SPEED = 5f;

    private static Sequence InternalFlashV2(Unit unit,
        SpriteRenderer[] renders,
        Color flash,
        bool onAndOffMode,
        float duration,
        TweenCallback onComplete,
        bool useUnitTimescale)
    {
        if (unit == null || renders == null)
            throw new Exception("Invalid flash request. Ya p-e quelque chose de null qui devrais pas.");

        Sequence sq = DOTween.Sequence().SetAutoKill(true);

        //Timescale
        if (useUnitTimescale)
            sq.timeScale = unit.TimeScale;

        //Delegates
        Unit.Unit_Event onTimeScaleChange = null;
        Unit.Unit_Event onUnitDeath = delegate (Unit u)
        {
            sq.Kill();
        };

        // Ajoute les listeners
        if (useUnitTimescale)
        {
            onTimeScaleChange = delegate (Unit u)
            {
                sq.timeScale = u.TimeScale;
            };
            unit.onTimeScaleChange += onTimeScaleChange;
        }
        unit.OnDeath += onUnitDeath;

        // Enleve les listeners
        sq.OnKill(delegate ()
        {
            if (useUnitTimescale)
                unit.onTimeScaleChange -= onTimeScaleChange;
            unit.OnDeath -= onUnitDeath;
        });


        // Quelques calculs
        int loops = duration > 0 ? Mathf.RoundToInt(duration * FLASH_SPEED) : -1;
        float actualFlashSpeed = duration > 0 ? loops / duration : FLASH_SPEED;

        Color[] stdColors = null;

        if (!onAndOffMode)
        {
            stdColors = new Color[renders.Length];
            for (int i = 0; i < renders.Length; i++)
            {
                stdColors[i] = renders[i].color;
            }
        }

        // ANIMATION 
        sq.InsertCallback(
            (1 / actualFlashSpeed) / 2,
            delegate
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    if (onAndOffMode)
                        renders[i].enabled = false;
                    else
                        renders[i].color = flash;
                }
            });
        sq.InsertCallback(
            1 / actualFlashSpeed,
            delegate
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    if (onAndOffMode)
                        renders[i].enabled = true;
                    else
                        renders[i].color = stdColors[i];
                }
            });

        //Loops
        sq.SetLoops(loops, LoopType.Restart);

        //On complete
        if (onComplete != null)
            sq.OnComplete(onComplete);

        return sq;
    }


    public static Sequence Flash(Unit unit, SpriteRenderer render, float duration, TweenCallback onComplete = null,
        bool useUnitTimescale = false)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        return Flash(unit, renders, duration, onComplete, useUnitTimescale);
    }

    public static Sequence Flash(Unit unit, SpriteRenderer[] renders, float duration, TweenCallback onComplete = null,
        bool useUnitTimescale = false)
    {
        Color dumbColor = Color.white;
        return InternalFlashV2(unit, renders, dumbColor, true, duration, onComplete, useUnitTimescale);
    }

    public static Sequence FlashInfinite(Unit unit, SpriteRenderer render, bool useUnitTimescale = false)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        return FlashInfinite(unit, renders, useUnitTimescale);
    }

    public static Sequence FlashInfinite(Unit unit, SpriteRenderer[] renders, bool useUnitTimescale = false)
    {
        Color dumbColor = Color.white;
        return InternalFlashV2(unit, renders, dumbColor, true, -1, null, useUnitTimescale);
    }

    public static Sequence FlashColor(Unit unit,
        SpriteRenderer render,
        float duration,
        Color flash,
        TweenCallback onComplete = null,
        bool useUnitTimescale = false)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        return InternalFlashV2(unit, renders, flash, false, duration, onComplete, useUnitTimescale);
    }

    public static Sequence FlashColor(Unit unit,
        SpriteRenderer[] renders,
        float duration,
        Color flash,
        TweenCallback onComplete = null,
        bool useUnitTimescale = false)
    {
        return InternalFlashV2(unit, renders, flash, false, duration, onComplete, useUnitTimescale);
    }

    public static Sequence FlashColorInfinite(Unit unit, SpriteRenderer render, Color flash, bool useUnitTimescale = false)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        return InternalFlashV2(unit, renders, flash, false, -1, null, useUnitTimescale);
    }

    public static Sequence FlashColorInfinite(Unit unit, SpriteRenderer[] renders, Color flash, bool useUnitTimescale = false)
    {
        return InternalFlashV2(unit, renders, flash, false, -1, null, useUnitTimescale);
    }
}
