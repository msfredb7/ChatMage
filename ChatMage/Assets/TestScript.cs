using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        Debug.LogWarning("Test script qui traine ici, ne m'oublier pas. (" + gameObject.name + ")");

        MasterManager.Sync();
    }

    [Range(0, 1)]
    public float volume;
    [Range(0,1)]
    public float overlap;
    [Range(0, 1)]
    public float startingVolume;


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //}
    }

    public void StopMusic()
    {
        SoundManager.StopMusic();
    }
    public void StopMusicFaded()
    {
        SoundManager.StopMusicFaded();
    }
    public void PlayMusic(AudioClip clip)
    {
        SoundManager.PlayMusic(clip, true, volume);
    }
    public void TransitionToMusic(AudioClip clip)
    {
        SoundManager.TransitionToMusic(clip, true, volume, overlap: overlap, startingVolume: startingVolume);
    }
    public void IsPlayingMusic()
    {
        print(SoundManager.IsPlayingMusic());
    }
}