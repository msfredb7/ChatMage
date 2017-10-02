using System.Collections;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;
using System;

[ExecuteInEditMode, RequireComponent(/*typeof(VignetteAndChromaticAberration),*/ typeof(Fisheye))]
public class ZaWarudoEffect : MonoBehaviour
{
    public Material material;
    [Header("All")]
    public float colorShiftStart = 0.2f;
    public float colorShiftend = 0.8f;
    public float fisheyeStrength = 0.35f;
    [Header("In")]
    public float appearDurationI = 0.5f;
    public float pauseDurationI = 1.5f;

    [Header("Out")]
    public float appearDurationO = 0.5f;
    public float pauseDurationO = 1.5f;

    public Fisheye fisheye;
    //public VignetteAndChromaticAberration vignette;
    private Tween tween;

    protected void Awake()
    {
        fisheye = GetComponent<Fisheye>();
        //vignette = GetComponent<VignetteAndChromaticAberration>();

        //vignette.chromaticAberration = 0;
        //vignette.blur = 0;

        fisheye.enabled = false;
        //vignette.enabled = false;
        enabled = false;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

    private void OnDestroy()
    {
        if (tween != null && tween.IsActive())
            tween.Kill();
    }

    public void Animate(TweenCallback apply)
    {
        ////Protection pour la fin de game
        //if (fisheye == null)
        //    return;

        enabled = true;
        fisheye.enabled = true;
        fisheye.strengthX = 0;
        fisheye.strengthY = 0;

        //vignette.enabled = true;
        //vignette.intensity = 0;

        float outerRayon = 0.0001f;
        float innerRayon = 0.0001f;
        float colorShift = colorShiftStart;
        material.SetFloat("_InvRayonE", 1 / innerRayon);
        material.SetFloat("_InvRayon", 1 / outerRayon);
        material.SetFloat("_ColShift", colorShift);

        Sequence sq = DOTween.Sequence();
        tween = sq;

        //Appear circle
        sq.Append(DOTween.To(
            () => outerRayon, //Getter
            delegate (float x)                    //Setter
            {
                outerRayon = x;
                material.SetFloat("_InvRayon", 1 / outerRayon);
            },
            1, //End Value
            appearDurationI) //Duration
        .SetEase(Ease.InQuad));

        //Color shift
        sq.Join(DOTween.To(
            () => colorShift, //Getter
            delegate (float x)                    //Setter
            {
                colorShift = x;
                material.SetFloat("_ColShift", colorShift);
            },
            colorShiftend, //End Value
            appearDurationI * 2 + pauseDurationI) //Duration
        .SetEase(Ease.Linear));

        //Fish eye in
        sq.Join(DOTween.To(
            () => fisheye.strengthX, //Getter
            delegate (float x)                    //Setter
            {
                fisheye.strengthX = x;
                fisheye.strengthY = x;
            },
            fisheyeStrength, //End Value
            appearDurationI + pauseDurationI) //Duration
        .SetEase(Ease.OutExpo));

        //Apply slow
        if (apply != null)
            sq.InsertCallback(appearDurationI, apply);

        //Fish eye out
        sq.Insert(appearDurationI + pauseDurationI,
            DOTween.To(
            () => fisheye.strengthX, //Getter
            delegate (float x)                    //Setter
            {
                fisheye.strengthX = x;
                fisheye.strengthY = x;
            },
            0, //End Value
            appearDurationI + pauseDurationI / 2) //Duration
        .SetEase(Ease.OutElastic).OnComplete(() =>
        {
            if (fisheye != null)
                fisheye.enabled = false;
        }));

        //Disapear circle
        sq.Insert(pauseDurationI + appearDurationI,
            DOTween.To(
            () => innerRayon, //Getter
            delegate (float x)                    //Setter
            {
                innerRayon = x;
                material.SetFloat("_InvRayonE", 1 / innerRayon);
            },
            1, //End Value
            appearDurationI) //Duration
        .SetEase(Ease.InQuad).OnComplete(() => enabled = false));

    }


    public void AnimateBack(TweenCallback unapply)
    {
        enabled = true;

        fisheye.enabled = true;
        fisheye.strengthX = 0;
        fisheye.strengthY = 0;

        float outerRayon = 1f;
        float innerRayon = 1f;
        float colorShift = colorShiftend;
        material.SetFloat("_InvRayonE", 1 / innerRayon);
        material.SetFloat("_InvRayon", 1 / outerRayon);
        material.SetFloat("_ColShift", colorShift);

        Sequence sq = DOTween.Sequence();
        tween = sq;

        //Appear circle
        sq.Append(DOTween.To(
            () => outerRayon, //Getter
            delegate (float x)                    //Setter
            {
                outerRayon = x;
                material.SetFloat("_InvRayonE", 1 / outerRayon);
            },
            0.0001f, //End Value
            appearDurationO) //Duration
        .SetEase(Ease.InQuad));

        //Color shift
        sq.Join(DOTween.To(
            () => colorShift, //Getter
            delegate (float x)                    //Setter
            {
                colorShift = x;
                material.SetFloat("_ColShift", colorShift);
            },
            colorShiftStart, //End Value
            appearDurationO * 2 + pauseDurationO) //Duration
        .SetEase(Ease.Linear));

        //Fish eye in
        sq.Join(DOTween.To(
            () => fisheye.strengthX, //Getter
            delegate (float x)                    //Setter
            {
                fisheye.strengthX = x;
                fisheye.strengthY = x;
            },
            fisheyeStrength, //End Value
            appearDurationO * 1.8f + pauseDurationO) //Duration
        .SetEase(Ease.OutExpo));

        //Apply slow
        if (unapply != null)
            sq.InsertCallback(appearDurationO, unapply);

        //Fish eye out
        sq.Insert(appearDurationO * 1.8f + pauseDurationO,
            DOTween.To(
            () => fisheye.strengthX, //Getter
            delegate (float x)                    //Setter
            {
                fisheye.strengthX = x;
                fisheye.strengthY = x;
            },
            0, //End Value
            appearDurationO + pauseDurationO / 2) //Duration
        .SetEase(Ease.OutElastic).OnComplete(() =>
        {
            if (fisheye != null)
                fisheye.enabled = false;
        }));

        //Disapear circle
        sq.Insert(pauseDurationO + appearDurationO,
            DOTween.To(
            () => innerRayon, //Getter
            delegate (float x)                    //Setter
            {
                innerRayon = x;
                material.SetFloat("_InvRayon", 1 / innerRayon);
            },
            0.0001f, //End Value
            appearDurationO) //Duration
        .SetEase(Ease.InQuad).OnComplete(() => enabled = false));
    }
}
