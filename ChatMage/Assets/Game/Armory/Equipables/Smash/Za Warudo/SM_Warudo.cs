using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using FullSerializer;
using DG.Tweening;
using UnityEngine.Audio;

public class SM_Warudo : Smash
{
    [InspectorHeader("Settings")]
    public float animDuration;
    public float timescaleMultiplier = 0.2f;
    public bool renderCarTrails = true;
    public float secondsPerJuiceUnits = 0.5f;

    [InspectorHeader("SFX Linking")]
    public AudioClip sfx;
    public AudioClip exitSfx;
    public AudioMixerSnapshot slowMotionSnapshot;
    public AudioMixerSnapshot normalSnapshot;

    [InspectorHeader("VFX Linking")]
    public Material zaWarudoMat;
    public Shader fishEyeShader;
    public CanvasGroup vignette;
    public TrailRenderer carTrails;
    public string trailsLayer;
    public int trailsOrderInLayer;

    [fsIgnore, NonSerialized]
    private ZaWarudoEffect vfx;
    [fsIgnore, NonSerialized]
    private CanvasGroup vignette_instance;
    [fsIgnore, NonSerialized]
    private Action onComplete;
    [fsIgnore, NonSerialized]
    private GameObject[] activeCarTrails = new GameObject[2];
    [fsIgnore, NonSerialized]
    private bool isIn = false;
    [fsIgnore, NonSerialized]
    private SmashManager smashManager;


    private const float zwv_ColorShiftStart = 0.2f;
    private const float zwv_ColorShiftEnd = 0.8f;
    private const float zwv_FishEyeStrength = 0.5f;

    private const float zwv_AppearDurationI = 0.35f;
    private const float zwv_PauseDurationI = 0.4f;

    private const float zwv_AppearDurationO = 0.35f;
    private const float zwv_PauseDurationO = 0.3f;

    public override void Init(PlayerController player)
    {
        base.Init(player);
        vfx = Game.Instance.gameCamera.cam.gameObject.AddComponent<ZaWarudoEffect>();
        vfx.colorShiftStart = zwv_ColorShiftStart;
        vfx.colorShiftend = zwv_ColorShiftEnd;
        vfx.fisheyeStrength = zwv_FishEyeStrength;
        vfx.material = zaWarudoMat;

        vfx.appearDurationI = zwv_AppearDurationI;
        vfx.pauseDurationI = zwv_PauseDurationI;

        vfx.appearDurationO = zwv_AppearDurationO;
        vfx.pauseDurationO = zwv_PauseDurationO;

        vfx.fisheye.fishEyeShader = fishEyeShader;

        vignette_instance = Instantiate(vignette.gameObject, Game.Instance.ui.stayWithinGameView).GetComponent<CanvasGroup>();
        vignette_instance.gameObject.SetActive(false);
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        for (int i = 0; i < activeCarTrails.Length; i++)
        {
            activeCarTrails[i] = null;
        }
        isIn = false;
        smashManager = null;
    }

    public override void OnGameReady()
    {
        player.vehicle.onDestroy += OnPlayerDestroy;
        smashManager = Game.Instance.smashManager;
    }

    void OnPlayerDestroy(Unit player)
    {
        //a-t-on simplement fermer le jeu ?
        if (Game.Instance != null && Application.isPlaying && isIn)
        {
            OnSmashEnd();
        }

        //On s'assure d'enlever l'effet
        if (isIn)
        {
            isIn = false;
            normalSnapshot.TransitionTo(0.75f);
            //SoundManager.SlowMotionEffect(false);
        }
    }

    public override void OnGameStarted()
    {

    }

    public override void OnSmash(Action onComplete)
    {
        this.onComplete = onComplete;
        isIn = false;

        //Vignette
        vignette_instance.gameObject.SetActive(true);
        vignette_instance.alpha = 0;
        vignette_instance.DOFade(1, 1);

        //Shader animation
        vfx.Animate(delegate ()
        {
            if (Game.Instance == null)
                return;

            slowMotionSnapshot.TransitionTo(0.75f);
            //SoundManager.SlowMotionEffect(true);

            isIn = true;

            MultiplyTimescale(timescaleMultiplier);
        },
        () =>
        {
            //Car trails
            DetachActiveCarTrails();
            if (renderCarTrails)
                AddActiveCarTrails();
        });

        DefaultAudioSources.PlayStaticSFX(sfx);
    }

    void MultiplyTimescale(float multiplier)
    {
        LinkedListNode<Unit> node = Game.Instance.units.First;
        while (node != null)
        {
            Unit val = node.Value;

            if (val.allegiance != Allegiance.Ally)
                val.TimeScale *= multiplier;

            node = node.Next;
        }

        if (multiplier > 1)
            Game.Instance.worldTimeScale.RemoveBuff("zwrdo");
        else
            Game.Instance.worldTimeScale.AddBuff("zwrdo", multiplier * 100 - 100, CCC.Utility.BuffType.Percent);
    }

    void AddActiveCarTrails()
    {
        if (Game.Instance == null || Game.Instance.Player == null)
            return;

        carTrails.sortingLayerName = trailsLayer;
        carTrails.sortingOrder = trailsOrderInLayer;

        PlayerLocations pl = Game.Instance.Player.playerLocations;
        activeCarTrails[0] = Instantiate(carTrails.gameObject, pl.BackLeftWheel.position, Quaternion.identity, pl.BackLeftWheel);
        activeCarTrails[0].transform.localPosition = new Vector3(-.1f, -.07f, 0);
        activeCarTrails[0].GetComponent<TrailRenderer>().enabled = true;

        activeCarTrails[1] = Instantiate(carTrails.gameObject, pl.BackRightWheel.position, Quaternion.identity, pl.BackRightWheel);
        activeCarTrails[1].transform.localPosition = new Vector3(-.1f, .07f, 0);
        activeCarTrails[1].GetComponent<TrailRenderer>().enabled = true;

    }
    void DetachActiveCarTrails()
    {
        if (Game.Instance == null)
            return;

        for (int i = 0; i < activeCarTrails.Length; i++)
        {
            if (activeCarTrails[i] == null)
                continue;

            activeCarTrails[i].transform.SetParent(Game.Instance.unitsContainer);
            activeCarTrails[i].GetComponent<TrailRenderer>().autodestruct = true;
        }
    }

    void OnSmashEnd()
    {
        if (!isIn)
            Debug.LogError("bug ? On sort du ZaWarudo sans y avoir enter d'abord.");

        if (onComplete != null)
            onComplete();
        onComplete = null;

        isIn = false;

        //Vignette
        vignette_instance.DOFade(0, 1).OnComplete(() =>
        {
            if (vignette_instance != null)
                vignette_instance.gameObject.SetActive(false);
        });

        //Shader animation
        DetachActiveCarTrails();
        vfx.AnimateBack(delegate ()
        {
            MultiplyTimescale(1 / timescaleMultiplier);
        });


        normalSnapshot.TransitionTo(0.75f);
        //SoundManager.SlowMotionEffect(false);
        DefaultAudioSources.PlayStaticSFX(exitSfx);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isIn && smashManager.CurrentJuice > 0)
        {
            smashManager.IncreaseSmashJuice(-Time.deltaTime / secondsPerJuiceUnits);

            if (smashManager.CurrentJuice <= 0)
                OnSmashEnd();
        }
    }
}
