using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using DG.Tweening;

public class ITM_SuperCar : Item, ISpeedBuff
{
    [InspectorHeader("Visuals")]
    public SuperCarVisualsController carVisual;

    [InspectorHeader("Settings")]
    public AnimationCurve speedCurve;
    public float nitroDuration;
    public float nitroSpeed;
    public float nitroPitchMultiplier;
    public float nitroVolumeMultiplier;

    [InspectorHeader("Control")]
    public FloatVariable remainingNitro;
    public FloatVariable nitroAudioTransition;
    public UnityObjectVariable controller;
    public GameEvent findNewControllerEvent;
    public UnityObjectVariable sharedAudioSource;
    public UnityObjectVariable sharedCarVisuals;

    [InspectorHeader("SFX")]
    public AudioPlayable nitroActivation;
    public float minVolume;
    public float maxVolume;
    public float minPitch;
    public float maxPitch;
    public AudioSource audioSourcePrefab;

    private SpriteRenderer originalCarVisuals;

    public override void Equip(int duplicateIndex)
    {
        base.Equip(duplicateIndex);

        originalCarVisuals = player.playerStats.sprite;

        AddBuff();

        // Spawn if null
        if (sharedAudioSource.Value == null)
        {
            SpawnAudio();
        }
        if (sharedCarVisuals.Value == null)
        {
            SpawnVisuals();
        }

        if (controller.Value == null)
            IsController = true;

        // Subscribe to events
        player.playerItems.OnGainItem += AddNitroBoost;
        findNewControllerEvent.Subscribe(FindNewAudioController);
    }

    public float GetAdditionalSpeed()
    {
        if (IsController)
            return remainingNitro > 0 ? nitroSpeed * speedCurve.Evaluate(1 - Mathf.Clamp01(remainingNitro / nitroDuration)) : 0;
        else
            return 0;
    }

    public override void Unequip()
    {
        base.Unequip();

        // Unsubscribe from events
        player.playerItems.OnGainItem -= AddNitroBoost;
        findNewControllerEvent.Unsubscribe(FindNewAudioController);

        // Lose control of source
        if (IsController)
        {
            IsController = false;
            findNewControllerEvent.Raise();

            // We're the last one, remove everything
            if (controller.Value == null)
            {
                DestroyAudio();
                DestroyVisuals();
                nitroAudioTransition.Value = 0;
            }
        }

        RemoveBuff();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (IsController)
        {
            // Transition
            nitroAudioTransition.Value = Mathf.MoveTowards(nitroAudioTransition.Value, remainingNitro > 0 ? 1 : 0, Time.deltaTime / 0.8f);

            // Audio
            var audioSource = sharedAudioSource.Value as AudioSource;
            var motorSFX = player.vehicleSounds;
            var motorSource = motorSFX.motorSource;

            float pitch01 = Mathf.Clamp01((motorSource.pitch - motorSFX.minPitch) / (motorSFX.maxPitch - motorSFX.minPitch));
            float volume01 = Mathf.Clamp01((motorSource.volume - motorSFX.minVolume) / (motorSFX.maxVolume - motorSFX.minVolume));

            audioSource.pitch = (minPitch + (pitch01 * (maxPitch - minPitch)));

            audioSource.volume = minVolume + (volume01 * (maxVolume - minVolume));
            audioSource.pitch *= Mathf.Lerp(1, nitroPitchMultiplier, nitroAudioTransition);
            audioSource.volume *= Mathf.Lerp(1, nitroVolumeMultiplier, nitroAudioTransition);

            // Visuals
            SharedCarVisualsController.SetSpriteActive(originalCarVisuals.enabled);
            SharedCarVisualsController.SetNitroPower01(Mathf.Clamp01(remainingNitro / nitroDuration));

            // Nitro
            remainingNitro.Value = Mathf.Max(remainingNitro.Value - Time.deltaTime, 0);
        }

    }

    private void AddNitroBoost(Item newItem)
    {
        Game.Instance.DelayedCall(() =>
        {
            if (IsController)
                DefaultAudioSources.PlaySFX(nitroActivation);
            remainingNitro.Value += nitroDuration;
        }, 0.5f);
    }

    private void AddBuff()
    {
        player.vehicle.speedBuffs.Add(this);
    }

    private void RemoveBuff()
    {
        player.vehicle.speedBuffs.Remove(this);
    }

    private SuperCarVisualsController SharedCarVisualsController
    {
        get { return (SuperCarVisualsController)sharedCarVisuals.Value; }
    }

    private void SpawnVisuals()
    {
        sharedCarVisuals.Value = carVisual.DuplicateGO(player.body);
        originalCarVisuals.gameObject.SetActive(false);
    }

    private void DestroyVisuals()
    {
        Destroy(SharedCarVisualsController.gameObject);
        originalCarVisuals.gameObject.SetActive(true);
    }

    private void SpawnAudio()
    {
        sharedAudioSource.Value = audioSourcePrefab.DuplicateGO();
    }
    private void DestroyAudio()
    {
        Destroy((sharedAudioSource.Value as AudioSource).gameObject);
    }

    private void FindNewAudioController()
    {
        if (controller.Value == null)
            IsController = true;
    }

    private bool IsController
    {
        get { return controller.Value == this; }
        set
        {
            if (value)
                controller.Value = this;
            else if (IsController)
                controller.Value = null;
        }
    }
}
