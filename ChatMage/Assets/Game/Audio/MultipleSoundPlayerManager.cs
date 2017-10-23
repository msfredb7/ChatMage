using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSoundPlayerManager : MonoBehaviour {

    public List<SoundPlayer> soundPlayers = new List<SoundPlayer>();

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
                soundPlayers[i].SetPlayerActive(false);
            }
        }
    }

    public void ActivateSoundPlayer(string tag)
    {
        for (int i = 0; i < soundPlayers.Count; i++)
        {
            if (soundPlayers[i].tag == tag)
            {
                soundPlayers[i].SetPlayerActive(true);
            }
        }
    }
}
