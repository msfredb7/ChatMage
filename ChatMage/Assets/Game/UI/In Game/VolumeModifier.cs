using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CCC.Manager;

public class VolumeModifier : MonoBehaviour
{
    public Slider sfxSlidder;
    public Slider musicSlidder;

    void Awake()
    {
        sfxSlidder.value = SoundManager.GetSFXSetting().dbBoost;
        sfxSlidder.onValueChanged.AddListener(delegate (float newValue)
        {
            SoundManager.SetSFX(newValue);
        });
        musicSlidder.value = SoundManager.GetMusicSetting().dbBoost;
        musicSlidder.onValueChanged.AddListener(delegate (float newValue)
        {
            SoundManager.SetMusic(newValue);
        });
    }
}