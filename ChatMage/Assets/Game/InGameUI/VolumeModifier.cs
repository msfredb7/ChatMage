using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour
{
    public Toggle sfxToggle;
    public Toggle musicToggle;

    void Awake()
    {
        sfxToggle.isOn = SoundManager.GetActiveSfx();
        sfxToggle.onValueChanged.AddListener(delegate (bool newValue)
        {
            SoundManager.SetActiveSFX(newValue);
        });
        musicToggle.isOn = SoundManager.GetActiveMusic();
        musicToggle.onValueChanged.AddListener(delegate (bool newValue)
        {
            SoundManager.SetActiveMusic(newValue);
        });
    }
}