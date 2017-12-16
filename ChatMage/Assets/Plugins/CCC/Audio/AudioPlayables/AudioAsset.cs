using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Asset")]
public class AudioAsset : AudioPlayable
{
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;


    protected override void Internal_PlayOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        audioSource.PlayOneShot(clip, volume * volumeMultiplier);
    }

    protected override void Interal_PlayLoopedOn(AudioSource audioSource, float volumeMultiplier = 1)
    {
        audioSource.volume = volumeMultiplier * volume;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public override void GetClipAndVolume(out AudioClip clip, out float volume)
    {
        clip = this.clip;
        volume = this.volume;
    }
}
