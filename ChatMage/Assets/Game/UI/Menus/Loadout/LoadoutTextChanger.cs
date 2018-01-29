using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LoadoutMenu
{
    public class LoadoutTextChanger : MonoBehaviour
    {
        [Header("Linking")]
        public Text text;

        [Header("Texts")]
        public string carText;
        public string smashText;
        public string itemsText;
        public string recapText;

        [Header("Optional audio")]
        public AudioClip carAudio;
        public AudioClip smashAudio;
        public AudioClip itemsAudio;
        public AudioClip recapAudio;

        public void SetCategory(LoadoutUI loadoutUI)
        {
            switch (loadoutUI.CurrentTab)
            {
                case LoadoutTab.Car:
                    text.text = carText;
                    DefaultAudioSources.PlaySFX(carAudio, volume: 1);
                    break;
                case LoadoutTab.Smash:
                    text.text = smashText;
                    DefaultAudioSources.PlaySFX(smashAudio, volume: 1);
                    break;
                case LoadoutTab.Items:
                    text.text = itemsText;
                    DefaultAudioSources.PlaySFX(itemsAudio, volume: 1);
                    break;
                case LoadoutTab.Recap:
                    text.text = recapText;
                    DefaultAudioSources.PlaySFX(recapAudio, volume: 1);
                    break;
                default:
                    throw new System.Exception("shit");
            }
        }
    }
}