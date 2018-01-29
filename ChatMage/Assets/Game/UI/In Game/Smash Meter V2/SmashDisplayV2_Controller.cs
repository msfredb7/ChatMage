using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashDisplayV2_Controller : MonoBehaviour
{
    public SmashDisplayV2 display;

    private SmashManager smashManager;
    private Game game;

    public void Init(PlayerController playerController)
    {
        smashManager = Game.Instance.smashManager;
        game = Game.Instance;

        smashManager.onJuiceChange += UpdateJuice;
        smashManager.onMinimumJuiceChange += UpdateMarker;
        smashManager.onMaxJuiceChange += UpdateJuiceAndMarker;
        smashManager.onEnableOrDisable += CheckIfSmashEnabled;

        game.ui.dialogDisplay.onStartDialog += HideBottle;
        game.ui.dialogDisplay.onClosingDialog += ShowBottle;

        UpdateJuiceAndMarker();

        display.Hide(false);
    }

    private void CheckIfSmashEnabled()
    {
        if (smashManager.IsEnabled)
            display.Show(true);
        else
            display.Hide(false);
    }

    private void ShowBottle()
    {
        display.Show(true);
    }

    private void HideBottle()
    {
        display.Hide(true);
    }

    private void UpdateJuiceAndMarker()
    {
        UpdateMarker();
        UpdateJuice();
    }

    private void UpdateJuice()
    {
        display.SetJuiceValue01(smashManager.CurrentJuice / smashManager.MaxJuice, game.gameReady);
    }

    private void UpdateMarker()
    {
        display.SetMarkerValue01(smashManager.MinimumActivatableJuice / smashManager.MaxJuice);
    }
}
