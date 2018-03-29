using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Music/Win"), DefaultNodeName("Win")]
    public class Win : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.Instance.music.TransitionTo(MusicManager.SongName.Win);
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }

        public override string NodeLabel()
        {
            return "Win Music for the end of the Game";
        }
    }
}
