using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class Fred_TestScript : BaseBehavior
{
    public AudioClip clip;
    public AudioSource source;
    public float volume;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            source.PlayOneShot(clip, volume);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            source.volume = volume;
            source.clip = clip;
            source.Play();
        }
    }
}