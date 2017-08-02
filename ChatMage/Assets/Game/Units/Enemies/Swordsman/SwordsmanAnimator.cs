using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordsmanAnimator : MonoBehaviour
{
    [Header("Linking")]
    public Transform spear;
    public SwordsmanVehicle veh;

    [Header("Settings")]
    [Header("Charge")]
    public float spearScaleDuration = 0.15f;
    public float chargeDuration = 0.75f;
    public float spearXCharge = -0.322f;
    public float spearXScale = 0.75f;

    [Header("Attack")]
    public Collider2D attackTrigger;
    [Range(0, 1)]
    public float enableTriggerAfter;
    public float spearXAttack = 1.25f;
    public float attackDuration = 0.75f;

    [Header("Retract")]
    public float pause = 0.75f;
    public float retractDuration = 0.25f;

    [Header("Armor loss")]
    public SpriteRenderer shell;
    public SpriteRenderer body;
    public float shellSizeIncrease;
    public float shellExplosionDuration;
    public Ease shellExplosionEase = Ease.OutSine;



    public Tween AttackAnimation(TweenCallback onAttackMoment)
    {
        float beginXScale = spear.localScale.x;

        //Launch anim
        Sequence sq = DOTween.Sequence();

        //Spear Scale
        sq.Join(
            spear.DOScaleX(spearXScale, spearScaleDuration)
            .SetEase(Ease.OutSine));

        //Spear Move
        sq.Join(
            spear.DOLocalMoveX(spearXCharge, chargeDuration)
            .SetEase(Ease.OutSine));

        //Activate trigger after x% of the attack move
        sq.InsertCallback(chargeDuration + attackDuration * enableTriggerAfter,
            delegate ()
            {
                veh.spearAttackConsumed = false;
                attackTrigger.enabled = true;
            });

        //Attack !
        sq.Append(
            spear.DOLocalMoveX(spearXAttack, attackDuration).SetEase(Ease.InSine));

        //Disable trigger
        sq.AppendCallback(delegate ()
        {
            attackTrigger.enabled = false;
            if (onAttackMoment != null)
                onAttackMoment();
        });

        //pause
        sq.AppendInterval(pause);

        //Retrack spear
        sq.Append(
            spear.DOLocalMoveX(0, retractDuration).SetEase(Ease.InOutSine));

        //Scale back to normal
        sq.Append(
            spear.DOScaleX(beginXScale, retractDuration).SetEase(Ease.InOutSine));

        return sq;
    }

    public Tween LoseArmor()
    {
        //Launch anim
        Sequence sq = DOTween.Sequence();

        body.enabled = true;
        sq.Join(shell.transform.DOScale(shell.transform.localScale.x * shellSizeIncrease, shellExplosionDuration)
            .SetEase(shellExplosionEase));
        sq.Join(shell.DOFade(0, shellExplosionDuration));
    
        return sq;
    }
}
