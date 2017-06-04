using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SmashBallAnimator : MonoBehaviour
{
    public SpriteRenderer[] ballRenderers;
    [Header("Unhittable"), Range(1, 20)]
    public float flashSpeed = 5;
    public float unhitableDuration = 1;

    SmashBall ball;
    Sequence unhitableTween;

    void Start()
    {
        ball = GetComponent<SmashBall>();
        ball.onHitPlayer += Ball_onHitPlayer;
        ball.onTimeScaleChange += Ball_onTimeScaleChange;
    }

    private void Ball_onTimeScaleChange(Unit unit)
    {
        if (unhitableTween != null)
            unhitableTween.timeScale = unit.TimeScale;
    }

    private void Ball_onHitPlayer()
    {
        if (ball.hp <= 0)
            return;
        
        ball.CanHit = false;

        unhitableTween = DOTween.Sequence();
        unhitableTween.InsertCallback(
            (1 / flashSpeed) / 2,
            delegate
            {
                SetVisible(false);
            });
        unhitableTween.InsertCallback(
            1 / flashSpeed,
            delegate
            {
                SetVisible(true);
            });
        unhitableTween.SetUpdate(false);
        unhitableTween.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
        unhitableTween.OnComplete(delegate ()
        {
            ball.CanHit = true;
        });
    }

    private void SetVisible(bool state)
    {
        for (int i = 0; i < ballRenderers.Length; i++)
        {
            ballRenderers[i].enabled = state;
        }
    }
}
