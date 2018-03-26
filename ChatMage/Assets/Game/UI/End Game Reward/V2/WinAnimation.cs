using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinAnimation : MonoBehaviour
{
    public PortalVFX portal;
    public RectTransform frame;
    public AudioAsset sfx;
    public CanvasGroup continueButton;

    private float frameFinalX;
    private Coroutine anim;

    private void Start()
    {
        var size = frame.sizeDelta;
        frameFinalX = size.x;
        continueButton.alpha = 0;
        continueButton.interactable = false;
        FlattenFrame();
    }

    private void FlattenFrame()
    {
        frame.sizeDelta = new Vector2(0, frame.sizeDelta.y);
    }

    public void Animate()
    {
        FlattenFrame();
        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        if (sfx != null)
            DefaultAudioSources.PlayStaticSFX(sfx);

        continueButton.alpha = 0;
        portal.Open();
        yield return new WaitForSeconds(portal.yDuration * 0.5f);
        frame.DOSizeDelta(new Vector2(frameFinalX, frame.sizeDelta.y), 1).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.75f);
        portal.Close();
        continueButton.interactable = true;
        continueButton.DOFade(1, 0.75f);
    }
}
