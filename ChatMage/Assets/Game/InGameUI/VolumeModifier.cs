using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour
{
    /*
    public Slider sfxSlider;
    public Slider musicSlider;
    */

    public Toggle sfxToggle;
    public Toggle musicToggle;

    void Awake()
    {
        /*
        sfxSlider.value = SoundManager.GetSfx();
        sfxSlider.onValueChanged.AddListener(OnEffetVolumeChange);
        musicSlider.value = SoundManager.GetMusic();
        musicSlider.onValueChanged.AddListener(OnMusiqueVolumeChange);
        */
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

    /*
    public void OnEffetVolumeChange(float newValue)
    {
        if (newValue <= sfxSlider.minValue)
        {
            SoundManager.SetSfx(-80);
        }
        else
        {
            SoundManager.SetSfx(newValue);
        }
    }

    public void OnMusiqueVolumeChange(float newValue)
    {
        if (newValue <= musicSlider.minValue)
        {
            SoundManager.SetMusic(-80);
        }
        else
        {
            SoundManager.SetMusic(newValue);
        }
    }
    */
}