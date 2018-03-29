using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMaestro : MonoBehaviour
{
    public AudioMixerSaver mixerSaver;
    private float wasMusicVolume;

    void Start()
    {
        wasMusicVolume = mixerSaver.GetVolume(AudioMixerSaver.ChannelType.Music);
        mixerSaver.SetVolume(AudioMixerSaver.ChannelType.Music, 1);
    }

    void OnDestroy()
    {
        if (mixerSaver != null && Application.isPlaying)
            mixerSaver.SetVolume(AudioMixerSaver.ChannelType.Music, wasMusicVolume);
    }
}
