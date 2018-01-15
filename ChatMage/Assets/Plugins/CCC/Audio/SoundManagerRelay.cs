using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerRelay : MonoBehaviour
{
    public void PlaySFX_AudioClip(AudioClip clip)
    {
        SoundManager.PlaySFX(clip);
    }
    public void PlaySFX_AudioPlayable(AudioPlayable playable)
    {
        SoundManager.PlaySFX(playable);
    }
    public void PlayVoice_AudioClip(AudioClip clip)
    {
        SoundManager.PlayVoice(clip);
    }
    public void PlayVoice_AudioPlayable(AudioPlayable playable)
    {
        SoundManager.PlayVoice(playable);
    }
    public void PlayStaticSFX_AudioClip(AudioClip clip)
    {
        SoundManager.PlayStaticSFX(clip);
    }
    public void PlayStaticSFX_AudioPlayable(AudioPlayable playable)
    {
        SoundManager.PlayStaticSFX(playable);
    }
    public void PlayMusic_AudioClip(AudioClip clip)
    {
        SoundManager.PlayMusic(clip);
    }
    public void PlayMusic_AudioPlayable(AudioPlayable playable)
    {
        SoundManager.PlayMusic(playable);
    }
}