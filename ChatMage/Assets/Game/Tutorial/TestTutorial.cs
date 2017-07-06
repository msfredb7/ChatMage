using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTutorial : BaseTutorial
{
    //private bool focusOnPlayer = false;
    public float duration = 5;

    public GameObject menuOption;

    /// <summary>
    /// Utiliser pour l'initialisation
    /// </summary>
    public override void Begin(GameObject canvas, LoadQueue queue)
    {
        base.Begin(canvas,queue);
        FocusInput(menuOption, false);
        ShowInfo("Voici le menu option ! Changer le volume, quitter la partie ou recommencer le niveau.", duration);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void End()
    {
        //focusOnPlayer = false;
        base.End();
    }
}
