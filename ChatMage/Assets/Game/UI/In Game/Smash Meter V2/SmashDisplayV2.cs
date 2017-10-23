using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashDisplayV2 : MonoBehaviour
{
    [SerializeField, Header("Juice")]
    private RectTransform juiceTr;
    [SerializeField]
    private float fullJuiceAncPos;
    [SerializeField]
    private float maximalJuiceAncPos = 0;
    [SerializeField]
    private float minimalJuiceAncPos = -460;
    [SerializeField]
    private float noJuiceAncPos = -460;
    [SerializeField]
    private float juiceMoveDuration = 0.5f;

    [SerializeField, Header("Marker")]
    private RectTransform markerTr;
    [SerializeField]
    private float highMarkerAncPos = -460;
    [SerializeField]
    private float lowMarkerAncPos;

    [SerializeField, Header("Glow Image")]
    private Image glower;
    [SerializeField]
    private Color glowColor = Color.white;
    [SerializeField]
    private Color noGlowColor = Color.black;
    [SerializeField]
    private float glowChangeDuration = 0.5f;

    [SerializeField, Header("Glow Transform")]
    private RectTransform glowTr;
    [SerializeField]
    private Vector2 fullGlowSize = new Vector2(-62, 430.9f);
    [SerializeField]
    private Vector2 noGlowSize = new Vector2(-62, 0);


    private Tween juiceMoveTween;
    private Tween glowChangeTween;
    private Tween glowSizeTween;
    private float juice01;
    private float marker01;
    private float glow01;
    private bool glowing = true;
    private const Ease JUICE_MOVE_EASE = Ease.OutCubic;


    public void SetJuiceValue01(float val, bool animated)
    {
        juice01 = val;

        //Determine le min/max
        float min = val > 0 ? minimalJuiceAncPos : noJuiceAncPos;
        float max = val < 1 ? maximalJuiceAncPos : fullJuiceAncPos;

        //Determine la destination
        Vector2 dest = new Vector2(0, min.Lerpped(max, val));

        //On tue le tween
        if (juiceMoveTween != null && juiceMoveTween.IsActive())
        {
            juiceMoveTween.Kill();
            juiceMoveTween = null;
        }

        //Animer ou non ?
        if (animated)
        {
            juiceMoveTween = juiceTr.DOAnchorPos(dest, juiceMoveDuration).SetEase(JUICE_MOVE_EASE);
        }
        else
        {
            juiceTr.anchoredPosition = dest;
        }

        SetGlowSize01(val, animated);

        CheckForGlow();
    }

    private void SetGlowSize01(float val, bool animated)
    {
        glow01 = val;

        //Determine le min/max
        Vector2 min = noGlowSize;
        Vector2 max = fullGlowSize;

        //Determine la destination
        Vector2 dest = min.Lerpped(max, val);

        //On tue le tween
        if (glowSizeTween != null && glowSizeTween.IsActive())
        {
            glowSizeTween.Kill();
            glowSizeTween = null;
        }

        //Animer ou non ?
        if (animated)
        {
            glowSizeTween = glowTr.DOSizeDelta(dest, juiceMoveDuration).SetEase(JUICE_MOVE_EASE);
        }
        else
        {
            glowTr.sizeDelta = dest;
        }

        CheckForGlow();
    }

    public void SetMarkerValue01(float val)
    {
        marker01 = val;

        Vector2 p = new Vector2(0, lowMarkerAncPos.Lerpped(highMarkerAncPos, val));
        markerTr.anchoredPosition = p;

        CheckForGlow();
    }

    void CheckForGlow()
    {
        bool shouldGlow = juice01 > 0 && marker01 <= juice01;

        if (glowing != shouldGlow)
        {
            SetGlow(shouldGlow);
        }
    }

    void SetGlow(bool state)
    {
        glowing = state;

        if (glowChangeTween != null && glowChangeTween.IsActive())
        {
            glowChangeTween.Kill();
            glowChangeTween = null;
        }

        if (state)
        {
            glower.enabled = true;
            glowChangeTween = glower.DOColor(glowColor, glowChangeDuration);
        }
        else
        {
            glowChangeTween = glower.DOColor(noGlowColor, glowChangeDuration).OnComplete(() => glower.enabled = false);
        }
    }
}
