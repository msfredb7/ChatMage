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


    public AudioClip clip;
    public AudioClip clip2;
    public AudioClip sfx;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SoundManager.PlayMusic(clip, true, 1, false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SoundManager.PlayMusic(clip2, true, 0.25f, false);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SoundManager.SetMusic(!SoundManager.GetMusicSetting().muted);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.SlowMotionEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SoundManager.SlowMotionEffect(false);
        }


        if (Input.GetKeyDown(KeyCode.Q))    //Master
        {
            //SoundManager.play(false);
        }
        if (Input.GetKeyDown(KeyCode.W))    //Voice
        {
            SoundManager.PlayVoice(sfx, 1);
        }
        if (Input.GetKeyDown(KeyCode.E))    //SFX
        {
            SoundManager.PlaySFX(sfx, 1);
        }
        if (Input.GetKeyDown(KeyCode.R))    //Staic SFX
        {
            SoundManager.PlayStaticSFX(sfx, 1);
        }
    }
}