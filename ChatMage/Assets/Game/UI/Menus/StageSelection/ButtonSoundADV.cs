using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundADV : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public AudioAsset onPress;
    public AudioAsset onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        DefaultAudioSources.PlayStaticSFX(onPress);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DefaultAudioSources.PlayStaticSFX(onClick);
    }
}
