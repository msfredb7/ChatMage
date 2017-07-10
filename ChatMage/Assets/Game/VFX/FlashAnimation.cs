using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashAnimation
{

    private static float flashSpeed = 5f;
    private static Sequence unhitableSequence;

    private static void InternalFlashV2(Unit unit,
        SpriteRenderer[] renders,
        Color flash,
        bool onAndOffMode,
        float duration,
        TweenCallback onComplete = null)
    {
        if (unit == null || renders == null || duration <= 0)
            throw new Exception("Invalid flash request. Ya p-e quelque chose de null qui devrais pas.");

        Sequence sq = DOTween.Sequence().SetAutoKill(true);

        //Timescale
        sq.timeScale = unit.TimeScale;

        //Delegates
        Unit.Unit_Event onTimeScaleChange = delegate (Unit u)
        {
            sq.timeScale = u.TimeScale;
        };
        Unit.Unit_Event onUnitDeath = delegate (Unit u)
        {
            sq.Kill();
        };

        // Ajoute les listeners
        unit.onTimeScaleChange += onTimeScaleChange;
        unit.onDeath += onUnitDeath;

        // Enleve les listeners
        sq.OnKill(delegate ()
        {
            unit.onTimeScaleChange -= onTimeScaleChange;
            unit.onDeath -= onUnitDeath;
        });


        // Quelques calculs
        int loops = Mathf.RoundToInt(duration * flashSpeed);
        float actualFlashSpeed = loops / duration;

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
    }


    public static void Flash(Unit unit, SpriteRenderer render, float duration, TweenCallback onComplete = null)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        Flash(unit, renders, duration, onComplete);
    }

    public static void Flash(Unit unit, SpriteRenderer[] renders, float duration, TweenCallback onComplete = null)
    {
        Color dumbColor = Color.white;
        InternalFlashV2(unit, renders, dumbColor, true, duration, onComplete);
    }

    public static void FlashColor(Unit unit,
        SpriteRenderer render,
        float duration,
        Color flash,
        TweenCallback onComplete = null)
    {
        SpriteRenderer[] renders = new SpriteRenderer[1];
        renders[0] = render;

        InternalFlashV2(unit, renders, flash, false, duration, onComplete);
    }

    public static void FlashColor(Unit unit,
        SpriteRenderer[] renders,
        float duration,
        Color flash,
        TweenCallback onComplete = null)
    {
        InternalFlashV2(unit, renders, flash, false, duration, onComplete);
    }
}
