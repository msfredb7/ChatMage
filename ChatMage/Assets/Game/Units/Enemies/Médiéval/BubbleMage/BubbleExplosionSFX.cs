using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleExplosionSFX : MonoBehaviour {

    public AudioPlayable bubbleExplosion;

	public void PlaySound()
    {
        DefaultAudioSources.PlaySFX(bubbleExplosion);
    }
}
