using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;
using CCC.Utility;

public class SoundPlayer : BaseBehavior
{
    public new string tag;

    public RandomAudioCliptList soundList;

    public enum SoundType { music = 0, sfx = 1, voice = 2 }
    public SoundType soundType = SoundType.sfx;

    [InspectorHeader("SETTINGS")]
    public bool useCustomSettings = false;
    [InspectorShowIf("useCustomSettings")]
    public bool loopingMusic = false;
    [InspectorShowIf("useCustomSettings")]
    public float volume = 1;
    [InspectorShowIf("useCustomSettings")]
    public float delay = 0;
    [InspectorShowIf("useCustomSettings")]
    public bool startOnReactivation = false;
    [InspectorShowIf("useCustomSettings")]
    public AudioSource sfxLoopSource;

    protected void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
    }

    public virtual void PlaySound()
    {
        switch (soundType)
        {
            case SoundType.music:
                DefaultAudioSources.PlayMusic(soundList.Pick(), loopingMusic, volume);
                break;
            case SoundType.sfx:
                DefaultAudioSources.PlaySFX(soundList.Pick(), delay, volume, sfxLoopSource);
                break;
            case SoundType.voice:
                DefaultAudioSources.PlayVoice(soundList.Pick(), delay, volume);
                break;
            default:
                break;
        }
    }

    public void SetLoopingSFXActive(bool state)
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
