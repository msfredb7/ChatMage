using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMaestro : MonoBehaviour
{
    public bool playFirstMusicOnStart = true;
    public AudioAsset[] musics;

    private int musicIndex = 0;

    void Start()
    {
        if (playFirstMusicOnStart)
        {
            DefaultAudioSources.PlayMusic(musics[musicIndex]);
            musicIndex++;
        }
    }

    public void NextMusic()
    {
        DefaultAudioSources.TransitionToMusic(musics[musicIndex]);
        musicIndex++;
    }
}
