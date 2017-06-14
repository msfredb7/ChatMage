using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using FullInspector;

public class SM_Warudo : Smash
{
    [InspectorHeader("Settings")]
    public float duration;
    public float targetTimeScale = 0;
    //public float launchDelay = 1.25f;

    [InspectorHeader("SFX Linking")]
    public AudioClip sfx;

    [InspectorHeader("VFX Linking")]
    public Material zaWarudoMat;
    public Shader fishEyeShader;
    public Shader vignetteShader;
    public Shader chromAberrationShader;
    public Shader separableBlurShader;


    private Coroutine smashCoroutine;
    private Coroutine smashLaunchCoroutine;
    private ZaWarudoEffect vfx;

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
        vfx.vignette.vignetteShader = vignetteShader;
        vfx.vignette.chromAberrationShader = chromAberrationShader;
        vfx.vignette.separableBlurShader = separableBlurShader;
    }

    public override void OnGameReady()
    {
        player.vehicle.onDestroy += OnPlayerDestroy;
    }

    void OnPlayerDestroy(Unit player)
    {
        //a-t-on simplement �teint le jeu ?
        if (DelayManager.instance == null)
            return;

        if (smashCoroutine != null)
        {
            DelayManager.Cancel(smashCoroutine);
            OnSmashEnd();
        }

        //if (smashLaunchCoroutine != null)
        //    DelayManager.Cancel(smashLaunchCoroutine);
    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash()
    {
        //smashLaunchCoroutine = DelayManager.CallTo(delegate ()
        //{
        vfx.Animate(delegate ()
        {
            SetTimeScale(targetTimeScale);

            smashCoroutine = DelayManager.CallTo(OnSmashEnd, duration);
        });
        //}, launchDelay);
        SoundManager.Play(sfx);
    }

    void SetTimeScale(float amount)
    {
        if (Game.instance == null)
            return;

        List<Unit> units = Game.instance.units;
        if (units == null)
            return;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].allegiance == Allegiance.Ally)
                continue;

            units[i].TimeScale = amount;
        }
        if (amount == 1)
            Game.instance.worldTimeScale.RemoveBuff("zwrdo");
        else
            Game.instance.worldTimeScale.AddBuff("zwrdo", amount * 100 - 100, CCC.Utility.BuffType.Percent);
    }

    void OnSmashEnd()
    {
        vfx.AnimateBack(delegate ()
        {
            smashCoroutine = null;
            SetTimeScale(1);
        });
    }

    public override void OnUpdate()
    {
    }
}
