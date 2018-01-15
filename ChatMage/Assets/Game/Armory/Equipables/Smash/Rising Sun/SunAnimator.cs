using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SunAnimator : MonoBehaviour, IShaker
{

    [Header("Camera Shake")]
    public float maxCamShakeReach = 1;
    public float maxCameraShake = 1f;

    [Header("Growth")]
    public float growthDuration = 0.5f;
    public Ease growthEase = Ease.OutBack;

    [Header("Shrink")]
    public float shrinkDuration = 0.5f;
    public Ease shrinkEase = Ease.OutBack;

    [Header("Screen Add")]
    public Color screenColor;
    public float screenColorReach = 0.5f;

    private VectorShaker camShake;
    private float currentSize = 0;
    private Tweener tween;
    private Transform tr;
    private ScreenAdd vfx;


    void Start()
    {
        camShake = Game.Instance.gameCamera.vectorShaker;
        camShake.AddShaker(this);
        tr = transform;
    }

    float SizeToShake()
    {
        float x = currentSize * maxCamShakeReach;
        float y = 1 - (1 / (x + 1));
        return maxCameraShake * y;
    }

    public void SetSize(float size, TweenCallback onComplete)
    {
        if (tween != null)
            tween.Kill();

        Ease ease = growthEase;
        float duration = growthDuration;

        if (size < currentSize)
        {
            ease = shrinkEase;
            duration = shrinkDuration;
        }

        tween = DOTween.To(() => currentSize,
            (x) =>
            {
                currentSize = x;
                tr.localScale = Vector3.one * currentSize;
                if (vfx != null)
                    vfx.color = Color.Lerp(Color.black, screenColor, 1 - (1 / (currentSize * screenColorReach + 1)));
            },
            size,
            duration)
            .SetEase(ease);

        if (onComplete != null)
            tween.OnComplete(onComplete);
    }


    public float GetShakeStrength()
    {
        return SizeToShake();
    }

    void OnDestroy()
    {
        if (camShake != null)
            camShake.RemoveShaker(this);
    }

    public void SetVFX(ScreenAdd vfx)
    {
        this.vfx = vfx;
    }
}
