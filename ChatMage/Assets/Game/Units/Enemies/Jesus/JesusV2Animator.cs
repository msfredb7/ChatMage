using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JesusV2Animator : MonoBehaviour
{
    [Header("Linking")]
    public Transform leftArm;
    public Transform rightArm;
    public Transform rockTransporter;

    [Header("Rock pick up")]
    public float pickup_rockTransporterBeginX;
    public float pickup_rockTransporterFinalX;
    public float pickup_armsExtendedXScale;
    public float pickup_extendDuration;
    public float pickup_retractDuration;

    [Header("Rock throw")]
    public float throw_rockTransporterBeginX;
    public float throw_rockTransporterFinalX;
    public float throw_armsExtendedXScale;
    public float throw_extendDuration;
    public float throw_retractDuration;



    public Tween PickUpRockAnimation(TweenCallback pickUpMoment)
    {
        Sequence sq = DOTween.Sequence();

        float stdXScale = leftArm.localScale.x;

        Vector2 pos = rockTransporter.localPosition;
        rockTransporter.localPosition = new Vector2(pickup_rockTransporterBeginX, pos.y);

        // On etand les bras vers l'avant
        sq.Join(leftArm.DOScaleX(pickup_armsExtendedXScale, pickup_extendDuration));
        sq.Join(rightArm.DOScaleX(pickup_armsExtendedXScale, pickup_extendDuration));

        sq.AppendCallback(pickUpMoment);

        //On retracte les bras
        sq.Append(leftArm.DOScaleX(stdXScale, pickup_retractDuration));
        sq.Join(rightArm.DOScaleX(stdXScale, pickup_retractDuration));

        //On bouge le transporteur
        sq.Join(rockTransporter.DOLocalMoveX(pickup_rockTransporterFinalX, pickup_retractDuration));

        return sq;
    }

    public Tween ThrowRockAnimation(TweenCallback throwMoment)
    {
        Sequence sq = DOTween.Sequence();

        float stdXScale = leftArm.localScale.x;

        Vector2 pos = rockTransporter.localPosition;
        rockTransporter.localPosition = new Vector2(throw_rockTransporterBeginX, pos.y);

        //On bouge le transporteur
        sq.Join(rockTransporter.DOLocalMoveX(throw_rockTransporterFinalX, throw_extendDuration));

        // On etand les bras vers l'avant
        sq.Join(leftArm.DOScaleX(throw_armsExtendedXScale, throw_extendDuration));
        sq.Join(rightArm.DOScaleX(throw_armsExtendedXScale, throw_extendDuration));

        sq.AppendCallback(throwMoment);

        //On retracte les bras
        sq.Append(leftArm.DOScaleX(stdXScale, throw_retractDuration));
        sq.Join(rightArm.DOScaleX(stdXScale, throw_retractDuration));

        return sq;
    }

    public Tween ScreamAnimation()
    {
        return null;
    }
}
