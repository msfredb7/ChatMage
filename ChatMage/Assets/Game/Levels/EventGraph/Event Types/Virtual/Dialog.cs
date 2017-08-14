using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class Dialog : VirtualEvent, IEvent
    {
        public const string NODE_NAME = "Dialog";

        public Dialoguing.Dialog dialog;

        public void Trigger()
        {
            if (dialog != null)
                Game.instance.ui.dialogDisplay.StartDialog(dialog);
        }

        public override string DefaultLabel()
        {
            return "Dialog: " + dialog.name;
        }
        public override Color DefaultColor()
        {
            return new Color(1, 1, 0.5f, 1);
        }
    }
}
