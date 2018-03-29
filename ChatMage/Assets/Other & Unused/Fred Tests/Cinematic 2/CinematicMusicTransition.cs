using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMusicTransition : MonoBehaviour
{
    public AudioAsset music;
    public bool smoothTransition = true;
    [ShowIf("smoothTransition")]
    public float fadingDuration = 1.5f;
    [ShowIf("smoothTransition")]
    public float overlap = 0.5f;
    [ShowIf("smoothTransition")]
    public float startingVolume = 0f;

    void OnEnable()
    {
        if (music == null)
            DefaultAudioSources.StopMusicFaded(fadingDuration);
        else
        {
            if (smoothTransition)
                DefaultAudioSources.TransitionToMusic(music, fadingDuration: fadingDuration, overlap: overlap, startingVolume: startingVolume);
            else
                DefaultAudioSources.PlayMusic(music);
        }
    }
}
