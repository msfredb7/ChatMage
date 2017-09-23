using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadingButton : MonoBehaviour
{
    [SerializeField, Header("Linking")]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField, Header("Animation")]
    private float fadeInDur = 0.25f;
    [SerializeField]
    private float fadeOutDur = 0.25f;

    public event SimpleEvent onClick;


    Tween currentTween;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (onClick != null)
            onClick();
    }

    public bool Interactable
    {
        get { return button.interactable; }
        set { button.interactable = value; }
    }


    public void Show(bool fadeIn = false)
    {
        if (currentTween != null)
            currentTween.Kill();

        button.enabled = true;
        gameObject.SetActive(true);

        if (fadeIn)
        {
            //Put alpha to 0
            SetAlphaTo(0);

            //Fade in
            currentTween = image.DOFade(1, fadeInDur);
        }
        else
        {
            //Alpha to 1
            SetAlphaTo(1);
        }
    }

    public void Hide(bool fadeOut = false)
    {
        if (currentTween != null)
            currentTween.Kill();

        button.enabled = false;

        if (fadeOut)
        {
            //Put alpha to 1
            SetAlphaTo(1);

            //Fade out, and hide quick afterwards
            currentTween = image.DOFade(0, fadeOutDur)
                .OnComplete(delegate ()
                {
                    gameObject.SetActive(false);
                });
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetAlphaTo(float amount)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, amount);
    }
}