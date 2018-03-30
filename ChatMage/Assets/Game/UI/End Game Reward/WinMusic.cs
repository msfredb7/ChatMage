using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinMusic : MonoBehaviour
{
    public bool playOnStart;
    public AudioSource introSource;
    public AudioSource loopSource;
    public float fadeOutDuration = 0.75f;
    public double loopDelay = 1.1986845;

    void Start()
    {
        if (playOnStart)
            Play();
    }

    private void Play()
    {
        introSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        loopSource.PlayScheduled(AudioSettings.dspTime + 0.1 + loopDelay);
    }

    public void FadeOut()
    {
        loopSource.DOFade(0, fadeOutDuration);
        introSource.DOFade(0, fadeOutDuration);
    }
}
