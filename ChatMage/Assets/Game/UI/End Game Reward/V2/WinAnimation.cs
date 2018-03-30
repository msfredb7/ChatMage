using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinAnimation : MonoBehaviour
{
    public PortalVFX portal;
    public RectTransform frame;
    public AudioAsset sfx;
    public float delayAfterSFX;
    public CanvasGroup buttons;

    private float frameFinalX;
    private Coroutine anim;

    private void Awake()
    {
        var size = frame.sizeDelta;
        frameFinalX = size.x;
        buttons.alpha = 0;
        buttons.interactable = false;
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
        buttons.alpha = 0;

        if (sfx != null)
            DefaultAudioSources.PlayStaticSFX(sfx);

        yield return new WaitForSeconds(delayAfterSFX);

        portal.Open();

        yield return new WaitForSeconds(portal.yDuration * 0.5f);
        frame.DOSizeDelta(new Vector2(frameFinalX, frame.sizeDelta.y), 1).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(0.75f);
        portal.Close();
        buttons.interactable = true;
        buttons.blocksRaycasts = true;
        buttons.DOFade(1, 0.75f);
    }
}
