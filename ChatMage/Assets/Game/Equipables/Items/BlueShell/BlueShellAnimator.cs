using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BlueShellAnimator : MonoBehaviour
{
    [Header("Other")]
    public SpriteRenderer shell;
    public BlueShellVehicle shellUnit;

    [Header("Explosion")]
    public LayerMask explosionLayerMask;
    public float explosionNormalRadius = 2;
    public float explosionBoostedRadius = 3;

    [Header("Core Animation")]
    public Animator coreAnimator;
    public float coreSizeMultiplier = 0.6f;
    public float coreBeginSize = 0.7f;
    public float coreGrowTime = 0.25f;
    public Ease coreEase = Ease.OutElastic;

    [Header("Light Animation")]
    public SpriteRenderer lightFade;
    public float lightSizeMultiplier = 0.8f;
    public float lightBeginopacity = 0.7f;
    public float lightIncreaseTime = 0.2f;

    [Header("Shockwave Animation")]
    public SpriteRenderer shockWave;
    public float shockWaveBeginSize = 0.25f;
    public float shockWaveEndSize = 8;
    public float shockWaveDuration = 0.35f;

    private Sequence tween;
    private bool atLeastAFrame = false;
    private float radius = 0;

    public void ResetValues()
    {
        shell.enabled = true;
        coreAnimator.gameObject.SetActive(false);
        lightFade.enabled = false;
        shockWave.enabled = false;
        atLeastAFrame = false;

        if (Game.instance.Player != null && Game.instance.Player.playerStats.boostedAOE)
            radius = explosionBoostedRadius;
        else
            radius = explosionNormalRadius;
    }

    public void Explode(TweenCallback onComplete)
    {
        //Enables
        shell.enabled = false;
        lightFade.enabled = true;
        shockWave.enabled = true;
        coreAnimator.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();

        //Grosseurs initiales
        coreAnimator.transform.localScale = Vector2.one * coreSizeMultiplier * coreBeginSize * radius;
        lightFade.transform.localScale = Vector2.one * lightSizeMultiplier * radius;
        shockWave.transform.localScale = Vector2.one * shockWaveBeginSize;
        Color stdColor = shockWave.color;
        shockWave.color = new Color(stdColor.r, stdColor.g, stdColor.b, 1);

        //animations
        sq.Join(coreAnimator.transform.DOScale(coreSizeMultiplier * radius, coreGrowTime).SetEase(coreEase));
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
        shellUnit.onTimeScaleChange += ShellUnit_onTimeScaleChange;
        ShellUnit_onTimeScaleChange(shellUnit);

        //Camera shake
        Game.instance.gameCamera.vectorShaker.Shake();

        //Explosion !
        Collider2D[] cols = Physics2D.OverlapCircleAll(shellUnit.Position, radius, explosionLayerMask);
        for (int i = 0; i < cols.Length; i++)
        {
            ColliderInfo otherInfo = cols[i].GetComponent<ColliderInfo>();
            if (otherInfo != null)
                UnitHit(otherInfo);
        }
    }

    private void UnitHit(ColliderInfo other)
    {
        if (other.parentUnit.allegiance != Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, shellUnit);
            }
        }
    }

    private void ShellUnit_onTimeScaleChange(Unit unit)
    {
        tween.timeScale = unit.TimeScale;
        coreAnimator.SetFloat("speed", unit.TimeScale);
    }
}
