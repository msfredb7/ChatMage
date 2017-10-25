using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class MusicManager : MonoBehaviour {

    public AudioClip ambient;
    public AudioClip fight;
    public AudioClip bossBattle;

    public enum SongName { Ambient = 0, Fight = 1, BossBattle = 2 }
    
    void Start()
    {
        Game.instance.onGameReady += delegate ()
        {
            PlaySong(SongName.Ambient);
        };
    }

    public void PlaySong(SongName song)
    {
        switch (song)
        {
            case SongName.Ambient:
                SoundManager.PlayMusic(ambient, true, 1);
                break;
            case SongName.Fight:
                SoundManager.PlayMusic(fight, true, 1);
                break;
            case SongName.BossBattle:
                SoundManager.PlayMusic(bossBattle, true, 1);
                break;
            default:
                break;
        }
    }

    public void TransitionTo(SongName song)
    {
        PlaySong(song);
    }
}
