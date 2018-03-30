using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMaestro : MonoBehaviour
{
    public AudioMixerSaver mixerSaver;
    private float wasMusicVolume;
    private bool wasMusicMuted;

    void Start()
    {
        wasMusicMuted = mixerSaver.GetMuted(AudioMixerSaver.ChannelType.Music);
        wasMusicVolume = mixerSaver.GetVolume(AudioMixerSaver.ChannelType.Music);
        mixerSaver.SetVolume(AudioMixerSaver.ChannelType.Music, 1);
        mixerSaver.SetMuted(AudioMixerSaver.ChannelType.Music, false);
    }

    void OnDestroy()
    {
        if (mixerSaver != null && Application.isPlaying)
        {
            mixerSaver.SetVolume(AudioMixerSaver.ChannelType.Music, wasMusicVolume);
            mixerSaver.SetMuted(AudioMixerSaver.ChannelType.Music, wasMusicMuted);
        }
    }
}
