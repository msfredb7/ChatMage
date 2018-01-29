using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    [MenuItem("Level/Dialog"), DefaultNodeName("Dialog")]
    public class Dialog : VirtualEvent, IEvent
    {
        public Dialoguing.Dialog dialog;
        public UnityEvent onComplete;

        public void Trigger()
        {
            if (dialog != null)
            {
                if (onComplete != null)
                    Game.Instance.ui.dialogDisplay.StartDialog(dialog);
                else
                    Game.Instance.ui.dialogDisplay.StartDialog(dialog, delegate() {
                        onComplete.Invoke();
                    });
            }

        }

        public override string NodeLabel()
        {
            if (dialog != null)
                return dialog.name;
            else
                return "Dialog";
        }
        public override Color GUIColor()
        {
            return Colors.DIALOG;
        }
    }
}
