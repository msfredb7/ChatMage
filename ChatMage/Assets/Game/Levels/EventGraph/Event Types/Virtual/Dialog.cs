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
            if (dialog != null)
                return "Dialog: " + dialog.name;
            else
                return "Dialog";
        }
        public override Color GUIColor()
        {
            return Colors.DIALOG;
        }
    }
}
