using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Win"), DefaultNodeName("Win")]
    public class Win : VirtualEvent, IEvent
    {
        public void Trigger()
        {
            Game.instance.levelScript.Win();
            Game.instance.music.TransitionTo(MusicManager.SongName.Win);
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }

        public override string NodeLabel()
        {
            return "Win the Game";
        }
    }
}
