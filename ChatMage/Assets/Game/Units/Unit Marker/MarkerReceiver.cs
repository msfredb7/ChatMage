using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerReceiver : MonoBehaviour
{
    [ReadOnlyInPlayMode]
    public SpriteRenderer[] renderers;
    public bool independantUpdate;

    [Header("Show animation")]
    public float preShow_Size = 0.25f;
    public float show_Size = 1;
    public Ease show_Ease = Ease.OutElastic;
    public float show_duration = 0.4f;

    [Header("Hide animation")]
    public float hide_Size = 2;
    public Ease hide_Ease = Ease.OutSine;
    public float hide_duration = 0.25f;

    public event SimpleEvent onHide;

    private Transform myTr;
    private Sequence sq;
    private float[] alphas;

    void Awake()
    {
        myTr = transform;
        CheckSetAlphas();
        gameObject.SetActive(false);
    }

    public void Show()
    {
        PreAnimate();
        gameObject.SetActive(true);

        myTr.localScale = Vector3.one * preShow_Size;
        sq.Join(myTr.DOScale(show_Size, show_duration).SetEase(show_Ease));

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].color = renderers[i].color.ChangedAlpha(0);

            sq.Join(renderers[i].DOFade(alphas[i], show_duration));
        }
    }

    public void Remove()
    {
        if (onHide != null)
        {
            onHide();
            onHide = null;
        }

        PreAnimate();

        sq.Join(myTr.DOScale(hide_Size, hide_duration).SetEase(hide_Ease));

        for (int i = 0; i < renderers.Length; i++)
        {
            sq.Join(renderers[i].DOFade(0, hide_duration));
        }

        sq.OnComplete(() => gameObject.SetActive(false));
    }

    void CheckSetAlphas()
    {
        if (alphas == null)
        {
            alphas = new float[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                alphas[i] = renderers[i].color.a;
            }
        }
    }

    void PreAnimate()
    {
        if (sq != null)
        {
            sq.Kill();
        }
        sq = DOTween.Sequence();
        sq.SetUpdate(independantUpdate);
    }
}
