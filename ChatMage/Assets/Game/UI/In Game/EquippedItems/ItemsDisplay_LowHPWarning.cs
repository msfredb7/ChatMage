using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsDisplay_LowHPWarning : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float flashDuration = 0.35f;
    [SerializeField] private Ease flashEase = Ease.InOutSine;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color warningColorA;
    [SerializeField] private Color warningColorB;
    [SerializeField] private Color fadeColor;//
    
    [Header("Fade"), SerializeField] private Image fadeImage;
    [SerializeField] private float fadeImageAlpha = 0.5f;

    private bool listenersAdded = false;
    private bool warningIsOn = false;
    private Tween currentAnimation;
    private Tween currentFadeAnimation;

    void Awake()
    {
        warningIsOn = true;
        SetWarning(false);
    }

    void Update()
    {
        if (listenersAdded)
            enabled = false;
        else
            AddListeners();
    }

    void AddListeners()
    {
        if (Game.Instance == null)
            return;
        if (Game.Instance.Player == null)
            return;

        Game.Instance.Player.playerItems.OnItemListChange += UpdateAnimation;
        listenersAdded = true;
    }

    public void SetWarning(bool on)
    {
        if (on == warningIsOn)
            return;

        fadeImage.enabled = on;

        Kill();
        if (on)
        {
            fadeImage.SetAlpha(0);
            currentFadeAnimation = fadeImage.DOFade(fadeImageAlpha, flashDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            currentAnimation = image.DOColor(warningColorA, flashDuration).SetEase(flashEase);
            currentAnimation.OnComplete(() =>
            {
                currentAnimation = image.DOColor(warningColorB, flashDuration).SetLoops(-1, LoopType.Yoyo).SetEase(flashEase);
            });
        }
        else
        {
            fadeImage.SetAlpha(0);
            image.DOColor(normalColor, flashDuration).SetEase(flashEase);
        }
        warningIsOn = on;
    }

    void UpdateAnimation()
    {
        if (Game.Instance.Player.playerItems.ItemCount == 0)
        {
            // Warning !!
            SetWarning(true);
        }
        else
        {
            // Player is Ok
            SetWarning(false);
        }
    }

    void Kill()
    {
        if (currentAnimation != null)
        {
            if (currentAnimation.IsActive())
                currentAnimation.Kill();
            currentAnimation = null;
        }
        if (currentFadeAnimation != null)
        {
            if (currentFadeAnimation.IsActive())
                currentFadeAnimation.Kill();
            currentFadeAnimation = null;
        }
    }
}
