using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTutorial : BaseTutorial {

    private bool focusOnPlayer = false;

    protected override void Start()
    {
        base.Start();
        if (TutorialStarter.tutorialScriptObject != null)
        {
            DelayManager.LocalCallTo(delegate ()
            {
                FocusInput(Game.instance.ui.menuOption.gameObject, true);
                ShowInfo("Voici le menu option ! Changer le volume, quitter la partie ou recommencer le niveau.");
            }, 1, TutorialStarter.tutorialScriptObject);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void End()
    {
        focusOnPlayer = false;
        base.End();
    }
}
