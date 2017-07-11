using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BardAnimator : MonoBehaviour
{
    [Header("Linking")]
    public SpriteRenderer sprite;

    [Header("Charge")]
    public Color chargeColor = Color.red;
    public float chargeDuration = 0.75f;

    [Header("Sing")]
    public SpriteRenderer[] singRenderers;
    public Color singColor = Color.yellow;
    public float singColorChangeDuration = 0.2f;
    public float singDuration = 1f;

    [Header("Back to normal")]
    public float backToNormalDuration = 0.25f;


    public Tween SingAnimation(TweenCallback onSingBegin, TweenCallback onSingEnd)
    {
        Sequence sq = DOTween.Sequence();

        //Charge
        sq.Append(sprite.DOColor(chargeColor, chargeDuration));

        //Sing start
        sq.AppendCallback(delegate()
        {
            SetSingRenderersState(true);
            onSingBegin();
        });

        sq.Append(sprite.DOColor(singColor, singColorChangeDuration));

        //Sing duration
        sq.AppendInterval(singDuration - singColorChangeDuration);

        sq.AppendCallback(delegate()
        {
            SetSingRenderersState(false);
            onSingEnd();
        });

        //Sing end
        sq.Append(sprite.DOColor(Color.white, backToNormalDuration));

        return sq;
    }

    private void SetSingRenderersState(bool state)
    {
        for (int i = 0; i < singRenderers.Length; i++)
        {
            singRenderers[i].enabled = state;
        }
    }
}
