using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimator : MonoBehaviour
{
    [Header("Core Animation")]
    public Animator coreAnimator;
    public float coreSizeMultiplier = 0.6f;
    public float coreBeginSize = 0.7f;
    public float coreGrowTime = 0.25f;
    public Ease coreEase = Ease.OutElastic;

    [Header("Shockwave Animation")]
    public SpriteRenderer shockWave;
    public float shockWaveBeginSize = 0.25f;
    public float shockWaveEndSize = 8;
    public float shockWaveDuration = 0.35f;

    private Sequence tween;

    public void Start()
    {
        coreAnimator.gameObject.SetActive(false);
        shockWave.enabled = false;
    }

    public void Explode(float radius, TweenCallback onComplete, Unit explodingUnit, float shakeStrength = 1)
    {
        //Enables
        shockWave.enabled = true;
        coreAnimator.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();

        //Grosseurs initiales
        coreAnimator.transform.localScale = Vector2.one * coreSizeMultiplier * coreBeginSize * radius;
        shockWave.transform.localScale = Vector2.one * shockWaveBeginSize;
        Color stdColor = shockWave.color;
        shockWave.color = new Color(stdColor.r, stdColor.g, stdColor.b, 1);

        //animations
        sq.Join(coreAnimator.transform.DOScale(coreSizeMultiplier * radius, coreGrowTime).SetEase(coreEase));
        sq.Join(shockWave.transform.DOScale(shockWaveEndSize, shockWaveDuration));
        sq.Join(shockWave.DOFade(0, shockWaveDuration).SetEase(Ease.InSine));

        sq.OnComplete(delegate ()
        {
            explodingUnit.onTimeScaleChange -= ShellUnit_onTimeScaleChange;
            if (onComplete != null)
                onComplete();
        });

        tween = sq;

        //Time scale
        explodingUnit.onTimeScaleChange += ShellUnit_onTimeScaleChange;
        ShellUnit_onTimeScaleChange(explodingUnit);

        //Camera shake
        if (shakeStrength > 0)
            Game.instance.gameCamera.vectorShaker.Shake(shakeStrength);
    }

    private void ShellUnit_onTimeScaleChange(Unit unit)
    {
        tween.timeScale = unit.TimeScale;
        coreAnimator.SetFloat("speed", unit.TimeScale);
    }
}
