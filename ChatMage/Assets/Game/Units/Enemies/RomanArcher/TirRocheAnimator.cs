using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TirRocheAnimator : MonoBehaviour
{
    [Header("Linking")]
    public TirRocheVehicle vehicle;
    public SpriteRenderer spriteRenderer;
    public TirRocheProjectile projectilePrefab;

    [Header("Reload")]
    public float reloadDuration = 0.25f;
    public Color clipFull;
    public Color clipEmpty;

    [Header("Charge")]
    public Transform arm;
    public float armRetractX = -0.5f;
    public float armRetractDuration = 0.35f;
    [Header("Shoot")]
    public float defaultShootDistance = 5;
    public float maxShootDistance = 5;
    public float playerPredictionAccuracy = 0.5f;
    public float armShootX = 1;
    public float armShootDuration = 0.15f;
    public float maxShootAngle = 50f;

    Tween currenTween;

    void Awake()
    {
        vehicle.onTimeScaleChange += Vehicle_onTimeScaleChange;
        spriteRenderer.color = Color.Lerp(clipEmpty, clipFull, ((float)vehicle.Ammo) / ((float)vehicle.maxAmmo));
    }

    private void Vehicle_onTimeScaleChange(Unit unit)
    {
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        if (currenTween != null)
            currenTween.timeScale = vehicle.TimeScale;
    }

    public void Reload(int newAmmoAmount, int maxAmmo, Action onComplete)
    {
        if (currenTween != null)
            currenTween.Kill();

        currenTween = spriteRenderer.DOColor(Color.Lerp(clipEmpty, clipFull, ((float)newAmmoAmount) / ((float)maxAmmo)), reloadDuration)
            .SetUpdate(false)
            .OnComplete(delegate ()
            {
                if (onComplete != null)
                    onComplete();
            });

        UpdateTimeScale();
    }

    public void Shoot(Unit target, Action onComplete)
    {
        if (currenTween != null)
            currenTween.Kill();

        Sequence sq = DOTween.Sequence();

        //charge
        sq.Append(arm.DOLocalMoveX(armRetractX, armRetractDuration).SetEase(Ease.OutSine));

        //Shoot
        sq.Append(arm.DOLocalMoveX(armShootX, armShootDuration).SetEase(Ease.InSine));
        sq.AppendCallback(delegate ()
        {
            Vector2 v = Vector2.zero;
            float dist = 0;
            if (target != null)
            {
                Vector2 delta = (Vector2)target.Position - vehicle.Position;

                float angleFromVehicle = Vehicle.VectorToAngle(delta) - vehicle.Rotation;

                if (Mathf.Abs(angleFromVehicle) > maxShootAngle)
                    delta = Vehicle.AngleToVector(vehicle.Rotation + Math.Sign(angleFromVehicle) * maxShootAngle) * delta.magnitude;

                Vector2 targetPos = target.Position + (target.Speed * delta.magnitude / projectilePrefab.shootSpeed) * playerPredictionAccuracy;

                v = targetPos - vehicle.Position;
                dist = v.magnitude;
            }
            else
            {
                v = vehicle.WorldDirection2D() * defaultShootDistance;
                dist = v.magnitude;
            }
            TirRocheProjectile proj = Game.instance.SpawnUnit(projectilePrefab, vehicle.Position);
            proj.Init(v, Math.Min(v.magnitude, maxShootDistance));
            proj.Init(vehicle);
            spriteRenderer.color = Color.Lerp(clipEmpty, clipFull, ((float)vehicle.Ammo) / ((float)vehicle.maxAmmo));
        });

        //retract
        sq.Append(arm.DOLocalMoveX(0, 0.25f).SetEase(Ease.InOutSine));

        sq.OnComplete(delegate ()
        {
            if (onComplete != null)
                onComplete();
        });

        sq.SetUpdate(false);

        currenTween = sq;

        UpdateTimeScale();
    }
}
