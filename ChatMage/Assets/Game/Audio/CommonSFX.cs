using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSFX : MonoBehaviour
{
    public List<AudioClip> hitSounds;

    public float sfxStandardVolume = 1;

    private int currentSoundIndex;

    void Start()
    {
        ShuffleHitSounds();
        currentSoundIndex = 0;
    }

    public void Hit()
    {
        SoundManager.PlaySFX(hitSounds[currentSoundIndex], 0, sfxStandardVolume);

        if ((currentSoundIndex + 1) >= hitSounds.Count)
            currentSoundIndex = 0;
        else
            currentSoundIndex++;
    }

    void ShuffleHitSounds()
    {
        for (int i = 0; i < hitSounds.Count; i++)
        {
            AudioClip temp = hitSounds[i];
            int randomIndex = Random.Range(i, hitSounds.Count);
            hitSounds[i] = hitSounds[randomIndex];
            hitSounds[randomIndex] = temp;
        }
    }
}
