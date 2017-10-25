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
        public MusicManager.SongName music;

        public void Trigger()
        {
            Game.instance.music.TransitionTo(music);
        }

        public override Color GUIColor()
        {
            return Colors.SOUND;
        }

        public override string NodeLabel()
        {
            return "Music Player";
        }
    }
}
