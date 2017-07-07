using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusAnimator : MonoBehaviour
{
    public JesusVehicle vehicle;
    public SpriteRenderer render;
    public float flashSpeed = 5;
    public float unhitableDuration = 1;

    public void OnHitFlashAnimation()
    {
        vehicle.canBeHit = false;

        Sequence unhitableSequence = DOTween.Sequence();
        unhitableSequence.InsertCallback(
            (1 / flashSpeed) / 2,
            delegate
            {
                render.enabled = false;
            });
        unhitableSequence.InsertCallback(
            1 / flashSpeed,
            delegate
            {
                render.enabled = true;
            });
        unhitableSequence.SetUpdate(false);
        unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
        unhitableSequence.OnComplete(delegate ()
        {
            vehicle.canBeHit = true;
        });
    }
}
