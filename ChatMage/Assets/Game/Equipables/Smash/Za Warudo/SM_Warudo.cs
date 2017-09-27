using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using FullInspector;
using FullSerializer;
using DG.Tweening;

public class SM_Warudo : Smash
{
    [InspectorHeader("Settings")]
    public bool useSmashCounter = false;
    public float duration;
    public float animDuration;
    public float timescaleMultiplier = 0.2f;
    //public float launchDelay = 1.25f;

    [InspectorHeader("SFX Linking")]
    public AudioClip sfx;

    [InspectorHeader("VFX Linking")]
    public Material zaWarudoMat;
    public Shader fishEyeShader;
    public CanvasGroup vignette;
    //public Shader vignetteShader;
    //public Shader chromAberrationShader;
    //public Shader separableBlurShader;

    [fsIgnore, NonSerialized]
    private Coroutine smashCoroutine;
    [fsIgnore, NonSerialized]
    private Coroutine smashLaunchCoroutine;
    [fsIgnore, NonSerialized]
    private ZaWarudoEffect vfx;
    [fsIgnore, NonSerialized]
    private CanvasGroup vignette_instance;
    [fsIgnore, NonSerialized]
    private Action onComplete;

    private const float zwv_ColorShiftStart = 0.2f;
    private const float zwv_ColorShiftEnd = 0.8f;
    private const float zwv_FishEyeStrength = 0.65f;

    private const float zwv_AppearDurationI = 0.4f;
    private const float zwv_PauseDurationI = 0.75f;

    private const float zwv_AppearDurationO = 0.4f;
    private const float zwv_PauseDurationO = 0.35f;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        vfx = Game.instance.gameCamera.cam.gameObject.AddComponent<ZaWarudoEffect>();
        vfx.colorShiftStart = zwv_ColorShiftStart;
        vfx.colorShiftend = zwv_ColorShiftEnd;
        vfx.fisheyeStrength = zwv_FishEyeStrength;
        vfx.material = zaWarudoMat;

        vfx.appearDurationI = zwv_AppearDurationI;
        vfx.pauseDurationI = zwv_PauseDurationI;

        vfx.appearDurationO = zwv_AppearDurationO;
        vfx.pauseDurationO = zwv_PauseDurationO;

        vfx.fisheye.fishEyeShader = fishEyeShader;
        //vfx.vignette.vignetteShader = vignetteShader;
        //vfx.vignette.chromAberrationShader = chromAberrationShader;
        //vfx.vignette.separableBlurShader = separableBlurShader;

        vignette_instance = Instantiate(vignette.gameObject, Game.instance.ui.stayWithinGameView).GetComponent<CanvasGroup>();
        vignette_instance.gameObject.SetActive(false);
    }

    public override void OnGameReady()
    {
        player.vehicle.onDestroy += OnPlayerDestroy;
    }

    void OnPlayerDestroy(Unit player)
    {
        if (smashCoroutine == null)
            return;

        //a-t-on simplement fermer le jeu ?
        if (Game.instance != null)
        {
            Game.instance.StopCoroutine(smashCoroutine);
            OnSmashEnd();
        }
    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash(Action onComplete)
    {
        this.onComplete = onComplete;

        //Vignette
        vignette_instance.gameObject.SetActive(true);
        vignette_instance.alpha = 0;
        vignette_instance.DOFade(1, 1);

        //Shader animation
        vfx.Animate(delegate ()
        {
            if (Game.instance == null)
                return;

            MultiplyTimescale(timescaleMultiplier);

            if(useSmashCounter)
                smashCoroutine = DelayManager.LocalCallTo(OnSmashEnd, Game.instance.smashManager.smashCounter + animDuration, Game.instance);
            else
                smashCoroutine = DelayManager.LocalCallTo(OnSmashEnd, duration + animDuration, Game.instance);
        });

        SoundManager.PlaySFX(sfx);
    }

    void MultiplyTimescale(float multiplier)
    {
        LinkedListNode<Unit> node = Game.instance.units.First;
        while (node != null)
        {
            Unit val = node.Value;

            if (val != player.vehicle)
                val.TimeScale *= multiplier;

            node = node.Next;
        }

        if (multiplier < 1)
            Game.instance.worldTimeScale.RemoveBuff("zwrdo");
        else
            Game.instance.worldTimeScale.AddBuff("zwrdo", multiplier * 100 - 100, CCC.Utility.BuffType.Percent);
    }

    void OnSmashEnd()
    {
        if (onComplete != null)
            onComplete();

        //Vignette
        vignette_instance.DOFade(0, 1).OnComplete(() =>
        {
            if (vignette_instance != null)
                vignette_instance.gameObject.SetActive(false);
        });

        //Shader animation
        vfx.AnimateBack(delegate ()
        {
            MultiplyTimescale(1 / timescaleMultiplier);
        });
        smashCoroutine = null;
    }

    public override void OnUpdate()
    {
    }
}
