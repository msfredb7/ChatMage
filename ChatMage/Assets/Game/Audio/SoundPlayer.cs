using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using CCC.Manager;
using UnityEngine.Events;

public class SoundPlayer : BaseBehavior {

    public string tag;

    [InspectorComment("This component only works with the library CCC.")]
    public bool useSoundList;
    [InspectorHideIf("useSoundList")]
    public AudioClip sound;
    [InspectorShowIf("useSoundList")]
    public List<AudioClip> soundList;

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

    public enum SoundType { music = 0, sfx = 1, voice = 2 }
    public SoundType soundType = SoundType.sfx;

    private int currentSoundIndex = 0;
    private bool active = true;

	void Start ()
    {
        MasterManager.Sync(delegate ()
        {
            active = true;
            currentSoundIndex = 0;
            if (onStart)
                PlaySound();
            if (useEvent)
                onPlaySound.AddListener(PlaySound);
            if (useSoundList)
                ShuffleList();
        });
	}

    void OnEnable()
    {
        if (onEnable)
            PlaySound();
    }

    public void PlaySound()
    {
        if (active)
        {
            switch (soundType)
            {
                case SoundType.music:
                    if (useSoundList)
                    {
                        SoundManager.PlayMusic(soundList[currentSoundIndex], looping, volume);

                        if ((currentSoundIndex + 1) >= soundList.Count)
                            currentSoundIndex = 0;
                        else
                            currentSoundIndex++;
                    }
                    else
                        SoundManager.PlayMusic(sound, looping, volume);
                    break;
                case SoundType.sfx:
                    if (useSoundList)
                    {
                        SoundManager.PlaySFX(soundList[currentSoundIndex], delay, volume);

                        if ((currentSoundIndex + 1) >= soundList.Count)
                            currentSoundIndex = 0;
                        else
                            currentSoundIndex++;
                    }
                    else
                        SoundManager.PlaySFX(sound, delay, volume);

                    if (looping)
                        DelayManager.LocalCallTo(PlaySound, sound.length + loopDelay, this);
                    break;
                case SoundType.voice:
                    if (useSoundList)
                    {
                        SoundManager.PlayVoice(soundList[currentSoundIndex], delay, volume);

                        if ((currentSoundIndex + 1) >= soundList.Count)
                            currentSoundIndex = 0;
                        else
                            currentSoundIndex++;
                    }
                    else
                        SoundManager.PlayVoice(sound, delay, volume);

                    if (looping)
                        DelayManager.LocalCallTo(PlaySound, sound.length + loopDelay, this);
                    break;
                default:
                    break;
            }
        } else
        {
            DelayManager.instance.StopAllCoroutines();
        }
    }

    void ShuffleList()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            AudioClip temp = soundList[i];
            int randomIndex = Random.Range(i, soundList.Count);
            soundList[i] = soundList[randomIndex];
            soundList[randomIndex] = temp;
        }
    }

    public void SetPlayerActive(bool state)
    {
        bool previousState = active;
        active = state;

        // Est ce qu'on avait arrêté de jouer le son ?
        if (previousState == false && active == true)
        {
            if(startOnReactivation) // Si oui on le refait a son activation si on veut
                PlaySound(); 
        }
    }
}
