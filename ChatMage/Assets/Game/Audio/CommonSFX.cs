using CCC.Manager;
using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSFX : MonoBehaviour
{
    public RandomAudioCliptList hitSounds;
    public RandomAudioCliptList smashActivation;

    public float sfxStandardVolume = 1;

    public void Hit()
    {
        SoundManager.PlaySFX(hitSounds.Pick(), 0, sfxStandardVolume);
    }

    public void SmashActive()
    {
        SoundManager.PlaySFX(smashActivation.Pick(), 0, sfxStandardVolume);
    }
}
