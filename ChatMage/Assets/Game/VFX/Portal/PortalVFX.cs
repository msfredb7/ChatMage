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

    private Sequence sq;

    void Awake()
    {
        Active(false);
    }

    void Start()
    {
        AddTimescaleListener();
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

        if (onComplete != null)
            sq.OnComplete(onComplete);
    }

    public void Open()
    {
        Open(null);
    }

    public void Close(TweenCallback onComplete)
    {
        sq.PlayBackwards();
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
}
