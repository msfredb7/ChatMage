using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashAnimation {

    private static float flashSpeed = 5f;
    private static Sequence unhitableSequence;

    public static void Flash(Unit unit, SpriteRenderer render, float unhitableDuration, Action onComplete)
    {
        unhitableSequence = DOTween.Sequence();
        unhitableSequence.timeScale = unit.TimeScale;
        unit.onTimeScaleChange += Unit_onTimeScaleChange;
        unit.onDeath += Unit_onDeath;
        unhitableSequence.InsertCallback(
            (1 / flashSpeed) / 2,
            delegate
            {
                render.enabled = false;
            });
        unhitableSequence.InsertCallback(
            1 / flashSpeed,
            delegate
            {
                render.enabled = true;
            });
        unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
        unhitableSequence.OnComplete(delegate ()
        {
            onComplete.Invoke();
        });
    }

    public static void FlashColor(Unit unit, SpriteRenderer render, float unhitableDuration, Color color, Action onComplete)
    {
        Color startColor = render.color;
        unhitableSequence = DOTween.Sequence();
        unhitableSequence.timeScale = unit.TimeScale;
        unit.onTimeScaleChange += Unit_onTimeScaleChange;
        unit.onDeath += Unit_onDeath;
        unhitableSequence.InsertCallback(
            (1 / flashSpeed) / 2,
            delegate
            {
                render.color = color;
            });
        unhitableSequence.InsertCallback(
            1 / flashSpeed,
            delegate
            {
                render.color = startColor;
            });
        unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
        unhitableSequence.OnComplete(delegate ()
        {
            onComplete.Invoke();
        });
    }

    public static void FlashMultiple(Unit unit, List<SpriteRenderer> renders, float unhitableDuration, Action onComplete)
    {
        for (int i = 0; i < renders.Count; i++)
        {
            unhitableSequence = DOTween.Sequence();
            unhitableSequence.timeScale = unit.TimeScale;
            unit.onTimeScaleChange += Unit_onTimeScaleChange;
            unit.onDeath += Unit_onDeath;
            unhitableSequence.InsertCallback(
                (1 / flashSpeed) / 2,
                delegate
                {
                    renders[i].enabled = false;
                });
            unhitableSequence.InsertCallback(
                1 / flashSpeed,
                delegate
                {
                    renders[i].enabled = true;
                });
            unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
            unhitableSequence.OnComplete(delegate ()
            {
                onComplete.Invoke();
            });
        }
    }

    public static void FlashMutipleColor(Unit unit, List<SpriteRenderer> renders, float unhitableDuration, Color color, Action onComplete)
    {
        for (int i = 0; i < renders.Count; i++)
        {
            Color startColor = renders[i].color;
            unhitableSequence = DOTween.Sequence();
            unhitableSequence.timeScale = unit.TimeScale;
            unit.onTimeScaleChange += Unit_onTimeScaleChange;
            unit.onDeath += Unit_onDeath;
            unhitableSequence.InsertCallback(
                (1 / flashSpeed) / 2,
                delegate
                {
                    renders[i].color = color;
                });
            unhitableSequence.InsertCallback(
                1 / flashSpeed,
                delegate
                {
                    renders[i].color = startColor;
                });
            unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
            unhitableSequence.OnComplete(delegate ()
            {
                onComplete.Invoke();
            });
        }
    }

    private static void Unit_onDeath(Unit unit)
    {
        unhitableSequence.Kill();
    }

    private static void Unit_onTimeScaleChange(Unit unit)
    {
        if(unhitableSequence != null)
            unhitableSequence.timeScale = unit.TimeScale;
    }
}
