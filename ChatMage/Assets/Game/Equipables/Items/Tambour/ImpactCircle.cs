using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ImpactCircle : MonoBehaviour
{
    [Header("Linking")]
    public SimpleColliderListener colliderListener;

    [Header("Settings")]
    public float endScaleValue;
    public float impactDuration = 1;

    void Start()
    {
        colliderListener.info.parentUnit = Game.instance.Player.vehicle;
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;

        //Begin scale
        transform.localScale = Vector2.one * 0.1f;

        //Grossit !
        transform.DOScale(endScaleValue, impactDuration)
            .OnComplete(delegate ()
            {
                Destroy(gameObject);
            });
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit is EnemyVehicle)
            (other.parentUnit as IAttackable).Attacked(other, 1, other.parentUnit);
    }
}
