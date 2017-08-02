using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TonyAnimator : MonoBehaviour
{
    public TonyVehicle vehicle;

    [Header("Pour reference a l'interne")]
    public float unitWidthWithZone = 1.75f;

    [Header("Zone")]
    public float zoneDuration = 3;
    public SpriteRenderer zoneSprite;
    public Collider2D zoneTrigger;

    [Header("Flash")]
    public SpriteRenderer flashSprite;
    public float flashDuration = 0.1f;
    public float flashAlpha = 0.7f;

    public float UnitWidthWithZone
    {
        get { return unitWidthWithZone; }
    }

    public Tween TakeSnap()
    {
        Sequence sq = DOTween.Sequence();

        zoneSprite.enabled = true;
        zoneTrigger.enabled = true;

        vehicle.onSnapTaken = delegate ()
        {
            Flash();
            sq.Complete();
        };

        sq.AppendInterval(zoneDuration);

        sq.OnComplete(delegate ()
        {
            //Close zone
            zoneSprite.enabled = false;
            zoneTrigger.enabled = false;

            //Remove listener
            vehicle.onSnapTaken = null;
        });

        return sq;
    }

    void Flash()
    {
        flashSprite.color = new Color(1, 1, 1, flashAlpha);
        flashSprite.enabled = true;
        flashSprite.DOFade(0, 0.1f).OnComplete(delegate () { flashSprite.enabled = false; });
    }
}
