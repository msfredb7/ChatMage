using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    void Awake()
    {
        sfxSlider.value = SoundManager.GetSfx();
        sfxSlider.onValueChanged.AddListener(OnEffetVolumeChange);
        musicSlider.value = SoundManager.GetMusic();
        musicSlider.onValueChanged.AddListener(OnMusiqueVolumeChange);
    }

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
}