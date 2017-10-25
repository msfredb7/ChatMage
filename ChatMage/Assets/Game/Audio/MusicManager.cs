using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using DG.Tweening;

public class MusicManager : MonoBehaviour {

    public AudioClip ambient;
    public AudioClip fight;
    public AudioClip bossBattle;
    public AudioClip winAnthem;

    public AudioSource transitionSource;

    public enum SongName { Ambient = 0, Fight = 1, BossBattle = 2, Win = 3 }

    public float seuilTransition = 0.25f;
    public float fadeDuration = 1;
    private bool soundManagerIsCurrent;

    void Start()
    {
        Game.instance.onGameReady += delegate ()
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
                {
                    transitionSource.volume = 1;
                    transitionSource.clip = ambient;
                    transitionSource.Play();
                    SoundManager.instance.musicSource.volume = 0;
                    soundManagerIsCurrent = false;
                } else
                {
                    SoundManager.PlayMusic(ambient, true, 1, false);
                    transitionSource.volume = 0;
                    soundManagerIsCurrent = true;
                }
                break;
            case SongName.Fight:
                if (transition)
                {
                    transitionSource.volume = 1;
                    transitionSource.clip = fight;
                    transitionSource.Play();
                    SoundManager.instance.musicSource.volume = 0;
                    soundManagerIsCurrent = false;
                }
                else
                {
                    SoundManager.PlayMusic(fight, true, 1, false);
                    transitionSource.volume = 0;
                    soundManagerIsCurrent = true;
                }
                break;
            case SongName.BossBattle:
                if (transition)
                {
                    transitionSource.volume = 1;
                    transitionSource.clip = bossBattle;
                    transitionSource.Play();
                    SoundManager.instance.musicSource.volume = 0;
                    soundManagerIsCurrent = false;
                }
                else
                {
                    SoundManager.PlayMusic(bossBattle, true, 1, false);
                    transitionSource.volume = 0;
                    soundManagerIsCurrent = true;
                }
                break;
            case SongName.Win:
                if (transition)
                {
                    transitionSource.volume = 1;
                    transitionSource.clip = winAnthem;
                    transitionSource.Play();
                    SoundManager.instance.musicSource.volume = 0;
                    soundManagerIsCurrent = false;
                }
                else
                {
                    SoundManager.PlayMusic(winAnthem, true, 1, false);
                    transitionSource.volume = 0;
                    soundManagerIsCurrent = true;
                }
                break;
            default:
                break;
        }
    }

    public void TransitionTo(SongName song)
    {
        if (!soundManagerIsCurrent)
        {
            transitionSource.DOFade(seuilTransition, fadeDuration).OnComplete(delegate() {
                PlaySong(song, true);
            });
        }
        else
        {
            SoundManager.instance.musicSource.DOFade(seuilTransition, fadeDuration).OnComplete(delegate () {
                PlaySong(song);
            });
        }
    }
}
