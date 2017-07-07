using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PinataVehicle : Unit, IAttackable
{
    //[Header("Linking")]
    //public SpriteRenderer sprite;
    //public Collider2D col;

    //[Header("Spawn")]
    //public float spawnSize;
    //public float spawnDuration = 0.25f;

    //[Header("Fall")]
    //public float targetSize;
    //public float fallDuration = 1;
    //public Ease fallEase = Ease.InSine;

    protected override void Awake()
    {
        base.Awake();
    }

    public void GoTo(Vector2 position)
    {
        Position = position;
        //sprite.color = new Color(1, 1, 1, 0);
        //sprite.DOFade(1, spawnDuration);
        //sprite.transform.localScale = Vector3.one * spawnSize;
        //sprite.transform.DOScale(targetSize, fallDuration).SetEase(fallEase);
    }

    public int Attacked(ColliderInfo on, int amount, Unit otherUnit, ColliderInfo source = null)
    {
        Die();
        return 0;
    }

    protected override void Die()
    {
        base.Die();

        Destroy();
    }
}
