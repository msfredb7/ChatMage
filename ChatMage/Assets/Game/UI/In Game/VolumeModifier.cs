using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class VolumeModifier : MonoBehaviour
{
    public Slider sfxSlidder;
    public Slider musicSlidder;

    void Awake()
    {
        sfxSlidder.value = AudioMixerSaves.Instance.SFX_dB;
        sfxSlidder.onValueChanged.AddListener(delegate (float newValue)
        {
            AudioMixerSaves.Instance.SFX_dB = newValue;
        });

        musicSlidder.value = AudioMixerSaves.Instance.Music_dB;
        musicSlidder.onValueChanged.AddListener(delegate (float newValue)
        {
            AudioMixerSaves.Instance.Music_dB = newValue;
        });
    }
}