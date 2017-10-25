using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using CCC.Manager;
using UnityEngine.Events;
using CCC.Utility;

public class SoundPlayer : BaseBehavior
{
    public new string tag;

    public RandomAudioCliptList soundList;

    [InspectorHeader("TIMING")]
    public bool onStart = false;
    public bool onEnable = false;
    public bool useEvent = false;
    [InspectorShowIf("useEvent")]
    public UnityEvent onPlaySound;

    [InspectorHeader("SETTINGS")]
    public bool useCustomSettings = false;
    [InspectorShowIf("useCustomSettings")]
    public bool looping = false;
    [InspectorShowIf("useCustomSettings")]
    public float loopDelay = 0;
    [InspectorShowIf("useCustomSettings")]
    public float volume = 1;
    [InspectorShowIf("useCustomSettings")]
    public float delay = 0;
    [InspectorShowIf("useCustomSettings")]
    public bool startOnReactivation = false;
    [InspectorShowIf("looping")]
    public AudioSource sfxLoopSource;

    public enum SoundType { music = 0, sfx = 1, voice = 2 }
    public SoundType soundType = SoundType.sfx;

    void Start()
    {
        MasterManager.Sync(delegate ()
        {
            if (onStart)
                PlaySound();
            if (useEvent)
                onPlaySound.AddListener(PlaySound);
        });
    }

    void OnEnable()
    {
        if (onEnable)
            PlaySound();
    }

    public void PlaySound()
    {
        switch (soundType)
        {
            case SoundType.music:
                SoundManager.PlayMusic(soundList.Pick(), looping, volume);
                break;
            case SoundType.sfx:
                SoundManager.PlaySFX(soundList.Pick(), delay, volume, sfxLoopSource);
                break;
            case SoundType.voice:
                SoundManager.PlayVoice(soundList.Pick(), delay, volume);
                break;
            default:
                break;
        }
    }

    public void SetSFXPlayerActive(bool state)
    {
        bool previousState = sfxLoopSource.enabled;
        sfxLoopSource.enabled = state;

        // Est ce qu'on avait arrêté de jouer le son ?
        if (previousState == false && state == true)
        {
            if (startOnReactivation) // Si oui on le refait a son activation si on veut
                PlaySound();
        }
    }
}
