using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Math;

[RequireComponent(typeof(PlayerDrift))]
public class PlayerDriftSounds : MonoBehaviour
{
    public float startAfterXs = 0.25f;
    public AudioSource driftSource;
    public float maxVolume;
    public float arriveSpeed;
    public float quitSpeed;

    private PlayerDrift playerDrift;
    private PlayerVehicle veh;

    private bool preventDriftSounds = false;

    void Start()
    {
        playerDrift = GetComponent<PlayerDrift>();
        veh = GetComponent<PlayerVehicle>();

        if (Game.instance.gameReady)
            AddListeners();
        else
            Game.instance.onGameReady += AddListeners;
    }

    void AddListeners()
    {
        DialogDisplay dd = Game.instance.ui.dialogDisplay;
        dd.onStartDialog += () => preventDriftSounds = true;
        dd.onClosingDialog += () => preventDriftSounds = false;
    }

    void Update()
    {
        if (driftSource == null)
        {
            enabled = false;
            return;
        }

        float targetVolume = playerDrift.GetDriftTime() > startAfterXs && !preventDriftSounds ? maxVolume : 0;
        float currentVolume = driftSource.volume;
        float speed = currentVolume < targetVolume ? arriveSpeed : quitSpeed;
        float deltaTime = preventDriftSounds ? Time.unscaledDeltaTime : veh.DeltaTime();

        driftSource.volume = currentVolume.MovedTowards(targetVolume, speed * deltaTime);
    }
}
