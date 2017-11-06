using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class MultipleSoundPlayerManager : BaseBehavior {

    public bool setListAutomaticaly = true;

    [InspectorHideIf("setListAutomaticaly")]
    public List<SoundPlayer> soundPlayers = new List<SoundPlayer>();

    void Start()
    {
        if (setListAutomaticaly)
        {
            if (soundPlayers == null)
                soundPlayers = new List<SoundPlayer>();

            SoundPlayer[] soundPlayerComponents = GetComponents<SoundPlayer>();
            for (int i = 0; i < soundPlayerComponents.Length; i++)
            {
                soundPlayers.Add(soundPlayerComponents[i]);
            }
        }
    }

	public void PlayChoosenSound(string tag)
    {
        for (int i = 0; i < soundPlayers.Count; i++)
        {
            if (soundPlayers[i].tag == tag) {
                if(soundPlayers[i].enabled)
                    soundPlayers[i].PlaySound();
            }
        }
    }

    public void DeactivateSoundPlayer(string tag)
    {
        for (int i = 0; i < soundPlayers.Count; i++)
        {
            if (soundPlayers[i].tag == tag)
            {
                soundPlayers[i].SetLoopingSFXActive(false);
            }
        }
    }

    public void ActivateSoundPlayer(string tag)
    {
        for (int i = 0; i < soundPlayers.Count; i++)
        {
            if (soundPlayers[i].tag == tag)
            {
                soundPlayers[i].SetLoopingSFXActive(true);
            }
        }
    }
}
