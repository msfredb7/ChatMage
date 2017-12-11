using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PortalVFX : InGameAnimator
{
    public Animator controller2;
    public Animator controller3;

    public Transform portalScaler;
    public new SpriteRenderer light;

    [Header("X")]
    public float xDuration;
    public Ease xEase;

    [Header("Y")]
    public float yDuration;
    public Ease yEase;

    [Header("Light")]
    public float lightAlpha;

    [Header("SFX")]
    public MultipleSoundPlayerManager soundPlayer;

    private Sequence sq;

    void Awake()
    {
        Active(false);
    }

    public void Open(TweenCallback onComplete)
    {
        Kill();

        Active(true);

        portalScaler.localScale = new Vector3(0, 0, 1);

        light.color = light.color.ChangedAlpha(0);

        sq = DOTween.Sequence();
        sq.SetAutoKill(false);
        sq.Join(portalScaler.DOScaleX(1, xDuration).SetEase(xEase));
        sq.Join(portalScaler.DOScaleY(1, yDuration).SetEase(yEase));
        sq.Join(portalScaler.DOScaleY(1, yDuration).SetEase(yEase));
        sq.Join(light.DOFade(lightAlpha, Mathf.Max(yDuration, xDuration)).SetEase(Ease.OutSine));

        AddTimescaleListener();

        if (soundPlayer != null)
        {
            soundPlayer.PlayChoosenSound("open");
            soundPlayer.GetSoundPlayer("loop").SetLoopingSFXActive(false);
        }

        sq.OnComplete(delegate ()
        {
            if (soundPlayer != null)
            {
                soundPlayer.GetSoundPlayer("loop").SetLoopingSFXActive(true);
                soundPlayer.PlayChoosenSound("loop");
            }
            if (onComplete != null)
                onComplete();
        });
    }

    public void Open()
    {
        Open(null);
    }

    public void Close(TweenCallback onComplete)
    {
        sq.PlayBackwards();
        soundPlayer.PlayChoosenSound("close");
        soundPlayer.GetSoundPlayer("loop").SetLoopingSFXActive(false);

        sq.OnComplete(() =>
        {
            Active(false);
            if (onComplete != null)
                onComplete();
        });
    }

    public void Close()
    {
        Close(null);
    }

    private void Kill()
    {
        if (sq != null && sq.IsActive())
            sq.Kill();
        sq = null;
    }

    void Active(bool state)
    {
        portalScaler.gameObject.SetActive(state);
        light.enabled = state;
    }

    protected override void UpdateTimescale(float worldTimescale)
    {
        base.UpdateTimescale(worldTimescale);
        controller2.speed = worldTimescale;
        controller3.speed = worldTimescale;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Kill();
    }
}
