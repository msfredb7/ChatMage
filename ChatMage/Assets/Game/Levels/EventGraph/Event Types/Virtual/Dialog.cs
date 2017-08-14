using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Dialog"), DefaultNodeName("Dialog")]
    public class Dialog : VirtualEvent, IEvent
    {
        public Dialoguing.Dialog dialog;

        public void Trigger()
        {
            if (dialog != null)
                Game.instance.ui.dialogDisplay.StartDialog(dialog);
        }

        public override string NodeLabel()
        {
            return "Dialog: " + dialog.name;
        }
        public override Color GUIColor()
        {
            return new Color(1, 1, 0.5f, 1);
        }
    }
}
