using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EndlessUI_WaveIntro : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Text text;
    [SerializeField] AudioPlayable majorSFX;
    [SerializeField] AudioPlayable minorSFX;

    [Header("Animation")]
    [SerializeField] float textScale = 1.25f;
    [SerializeField] float textScaleDuration = 2.5f;
    [SerializeField] Ease textScaleEase = Ease.Linear;
    [SerializeField] float textFadeInDuration = 0.5f;
    [SerializeField] float textFadeOutDuration = 0.85f;

    void Awake()
    {
        text.enabled = false;
    }

    public void ShowWaveIntro(int totalStep) { ShowWaveIntro(totalStep, null); }
    public void ShowWaveIntro(int totalStep, TweenCallback onComplete)
    {
        int level = Mathf.CeilToInt((totalStep) / 10f);
        int step = totalStep % 10;
        if (step == 0)
            step = 10;
        ShowWaveIntro(level, step, onComplete);
    }
    public void ShowWaveIntro(int level, int step) { ShowWaveIntro(level, step, null); }
    public void ShowWaveIntro(int level, int step, TweenCallback onComplete)
    {
        if (step == 1 || step == 10)
            DefaultAudioSources.PlayStaticSFX(majorSFX);
        else
            DefaultAudioSources.PlayStaticSFX(minorSFX);

        text.enabled = true;
        text.text = "Level " + level + " - " + (step == 10 ? "Finale" : step.ToString());

        // Fade in/out
        text.color = text.color.ChangedAlpha(0);
        text.DOFade(1, textFadeInDuration);
        text.DOFade(0, textFadeOutDuration).SetDelay(textScaleDuration - textFadeOutDuration);

        // Scale
        var rectT = text.rectTransform;
        rectT.localScale = Vector3.one;
        rectT.DOScale(textScale, textScaleDuration).SetEase(textScaleEase).onComplete = () =>
        {
            text.enabled = false;
            if (onComplete != null)
                onComplete();
        };
    }
}
