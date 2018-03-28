using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSFX : MonoBehaviour
{
    public RandomAudioCliptList hitSounds;
    public RandomAudioCliptList smashActivation;
    public RandomAudioCliptList deathSounds;
    public AudioClip winIntro;

    public float sfxStandardVolume = 1;

    public void Hit()
    {
        DefaultAudioSources.PlaySFX(hitSounds.Pick(), 0, sfxStandardVolume);
    }

    public void SmashActive()
    {
        DefaultAudioSources.PlaySFX(smashActivation.Pick(), 0, sfxStandardVolume);
    }

    public void Death()
    {
        DefaultAudioSources.PlaySFX(deathSounds.Pick(), 0, sfxStandardVolume);
    }

    public void WinEndGameIntro()
    {
        DefaultAudioSources.PlaySFX(winIntro, 0, 0.25f);
    }
}
