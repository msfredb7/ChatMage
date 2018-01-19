using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour {

    public AudioClip ambient;
    public AudioClip fight;
    public AudioClip bossBattle;
    public AudioClip winAnthem;

    public enum SongName { Ambient = 0, Fight = 1, BossBattle = 2, Win = 3 }

    public float transistionDuration = 1;
    private const float musicVolume = 0.15f;

    void Start()
    {
        Game.Instance.onGameReady += delegate ()
        {
            PlaySong(SongName.Ambient);
        };
    }

    public void PlaySong(SongName song, bool transition = false)
    {
        switch (song)
        {
            case SongName.Ambient:
                if (transition)
                    DefaultAudioSources.TransitionToMusic(ambient, true, musicVolume, transistionDuration);
                else
                    DefaultAudioSources.PlayMusic(ambient, true, musicVolume);
                break;
            case SongName.Fight:
                if (transition)
                    DefaultAudioSources.TransitionToMusic(fight, true, musicVolume,transistionDuration);
                else
                    DefaultAudioSources.PlayMusic(fight, true, musicVolume);
                break;
            case SongName.BossBattle:
                if (transition)
                    DefaultAudioSources.TransitionToMusic(bossBattle, true, musicVolume,transistionDuration);
                else
                    DefaultAudioSources.PlayMusic(bossBattle, true, musicVolume);
                break;
            case SongName.Win:
                if (transition)
                    DefaultAudioSources.TransitionToMusic(winAnthem, true, musicVolume,transistionDuration);
                else
                    DefaultAudioSources.PlayMusic(winAnthem, true, musicVolume);
                break;
            default:
                break;
        }
    }

    public void TransitionTo(SongName song)
    {
        PlaySong(song, true);
    }
}
