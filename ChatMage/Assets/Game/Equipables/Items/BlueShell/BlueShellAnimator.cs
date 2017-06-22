using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BlueShellAnimator : MonoBehaviour
{
    public BlueShellScript shellUnit;
    public Transform core;
    public SpriteRenderer shell;
    public float coreSizeMultiplier = 0.6f;
    public SpriteRenderer lightFade;
    public float lightSizeMultiplier = 0.8f;
    public SpriteRenderer shockWave;
    public Animator coreAnimator;

    [Header("Settings")]
    //public float coreDuration = 1;
    public float coreBeginSize = 0.7f;
    public float coreGrowTime = 0.25f;
    public Ease coreEase = Ease.OutElastic;
    public float lightBeginopacity = 0.7f;
    public float lightIncreaseTime = 0.2f;
    public float shockWaveBeginSize = 0.25f;
    public float shockWaveEndSize = 8;
    public float shockWaveDuration = 0.35f;

    private Sequence tween;

    public void ResetValues()
    {
        shell.enabled = true;
        core.gameObject.SetActive(false);
        lightFade.enabled = false;
        shockWave.enabled = false;
    }

    public void Explode(float size, TweenCallback onComplete)
    {
        //Enables
        shell.enabled = false;
        lightFade.enabled = true;
        shockWave.enabled = true;
        core.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();

        //Grosseurs initiales
        core.localScale = Vector2.one * coreSizeMultiplier * coreBeginSize * size;
        lightFade.transform.localScale = Vector2.one * lightSizeMultiplier * size;
        shockWave.transform.localScale = Vector2.one * shockWaveBeginSize;
        Color stdColor = shockWave.color;
        shockWave.color = new Color(stdColor.r, stdColor.g, stdColor.b, 1);

        //animations
        sq.Join(core.DOScale(coreSizeMultiplier * size, coreGrowTime).SetEase(coreEase));
        sq.Join(lightFade.DOFade(1, lightIncreaseTime));
        sq.Join(shockWave.transform.DOScale(shockWaveEndSize, shockWaveDuration));
        sq.Join(shockWave.DOFade(0, shockWaveDuration).SetEase(Ease.InSine));
        sq.Append(lightFade.DOFade(0, 1 - lightIncreaseTime).SetEase(Ease.InSine));

        sq.OnComplete(delegate ()
        {
            coreAnimator.SetTrigger("End");
            if (onComplete != null)
                onComplete();
        });

        tween = sq;

        //Time scale
        shellUnit.onTimeScaleChange += ShellUnit_onTimeScaleChange; ;

        //Camera shake
        Game.instance.gameCamera.vectorShaker.Shake();
    }

    private void ShellUnit_onTimeScaleChange(Unit unit)
    {
        tween.timeScale = unit.TimeScale;
    }
}
