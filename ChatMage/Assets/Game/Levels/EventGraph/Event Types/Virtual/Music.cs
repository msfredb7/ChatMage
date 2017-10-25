using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using CCC.Utility;

namespace GameEvents
{
    [MenuItem("Sound/Music Player"), DefaultNodeName("Music Player")]
    public class Music : VirtualEvent, IEvent
    {
        public RandomAudioCliptList soundList;

        public bool musicLoop = false;
        public bool musicFadePrevious = true;
        public float volume = 1;

        public SoundPlayer.SoundType soundType = SoundPlayer.SoundType.sfx;

        public void Trigger()
        {
            Play(soundList.Pick());
        }

        public override Color GUIColor()
        {
            return Colors.SOUND;
        }

        public override string NodeLabel()
        {
            return "Music Player";
        }

        void Play(AudioClip sound)
        {
            switch (soundType)
            {
                case SoundPlayer.SoundType.music:
                    SoundManager.PlayMusic(sound, musicLoop, volume, musicFadePrevious);
                    break;
                case SoundPlayer.SoundType.sfx:
                    SoundManager.PlaySFX(sound);
                    break;
                case SoundPlayer.SoundType.voice:
                    SoundManager.PlayVoice(sound);
                    break;
                default:
                    break;
            }
        }
    }
}
