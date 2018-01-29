using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSFX : MonoBehaviour
{
    public RandomAudioCliptList hitSounds;
    public RandomAudioCliptList smashActivation;
    public RandomAudioCliptList deathSounds;

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
}
