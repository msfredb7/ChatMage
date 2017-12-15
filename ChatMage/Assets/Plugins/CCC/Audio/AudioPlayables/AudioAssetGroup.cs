﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Audio/Audio Asset Group")]
public class AudioAssetGroup : AudioPlayable
{
    public AudioAsset[] clips;

    [NonSerialized]
    private int lastPickedIndex = -1;

    public override void PlayOn(AudioSource audioSource)
	{
		if (!CheckRessources())
            return;

        PickAsset().PlayOn(audioSource);
	}

	public override void PlayLoopedOn(AudioSource audioSource, bool multiplyVolume = false)
	{
		if (!CheckRessources())
			return;

        PickAsset().PlayLoopedOn(audioSource);
	}

	private bool CheckRessources(){
		return clips != null && clips.Length != 0;
	}

    private AudioAsset PickAsset()
    {
        if (lastPickedIndex >= clips.Length)
            lastPickedIndex = 0;

        if (clips.Length == 1)
            return clips[0];


        int from;
        int to;
        if (lastPickedIndex == -1)
        {
            //On a jamais pick
            from = 0;
            to = clips.Length;
        }
        else
        {
            //On a deja pick
            from = lastPickedIndex + 1;
            to = lastPickedIndex + clips.Length;
        }

        int pickedIndex = UnityEngine.Random.Range(from, to) % clips.Length;
        lastPickedIndex = pickedIndex;
        return clips[pickedIndex];
    }

    public override void GetClipAndVolume(out AudioClip clip, out float volume)
    {
        PickAsset().GetClipAndVolume(out clip, out volume);
    }
}
