using CCC.Manager;
using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSFX : MonoBehaviour
{
    public RandomAudioCliptList hitSounds;

    public float sfxStandardVolume = 1;

    public void Hit()
    {
        SoundManager.PlaySFX(hitSounds.Pick(), 0, sfxStandardVolume);
    }
}
