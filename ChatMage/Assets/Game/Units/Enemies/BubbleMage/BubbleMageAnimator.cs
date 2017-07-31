using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMageAnimator : MonoBehaviour
{
    public SpriteRenderer bodySprite;
    public SpriteRenderer directionSprite;

    [Header("Charge")]
    public Color toChargeColor;
    public float chargeDuration;

    [Header("Shoot")]
    public float spriteFinalX;
    public float spriteBeginX;
    public Color toShootColor;
    public float shootDuration;
    public float retractDuration;


    public Tween Charge()
    {
        return bodySprite.DOColor(toChargeColor, chargeDuration);
    }
    public Tween Shoot(TweenCallback shootMoment)
    {
        Sequence sq = DOTween.Sequence();

        //Sprite color
        sq.Join(bodySprite.DOColor(toShootColor, shootDuration));

        //Sprite move
        sq.Join(directionSprite.transform.DOLocalMoveX(spriteFinalX, shootDuration).SetEase(Ease.OutSine));

        //Shoot moment callback
        sq.InsertCallback(0, shootMoment);

        //Retract
        sq.Append(directionSprite.transform.DOLocalMoveX(spriteBeginX, retractDuration).SetEase(Ease.InSine));

        return sq;
    }
}
