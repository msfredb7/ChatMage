using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Wormhole : Unit {

    [InspectorHeader("Child")]
    public GameObject spriteNcollider;

    [InspectorHeader("Animation")]
    public Animator animations;
    public float wormholeFinalSize = 5;
    public Ease wormholeGrowEase = Ease.OutBack;
    public float rotationSpeed = 50;

    private float counter = 0;
    private bool counterStarted = false;

    void Start()
    {
        spriteNcollider.transform.DOScale(wormholeFinalSize, 4).SetEase(wormholeGrowEase);
        spriteNcollider.transform.DORotate(Vector3.back * 360, rotationSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        spriteNcollider.GetComponent<SimpleColliderListener>().onTriggerEnter += Wormhole_onTriggerEnter;
    }

    private void Wormhole_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if (other.parentUnit.allegiance != Allegiance.Ally)
        {
            IAttackable attackable = other.parentUnit.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.Attacked(other, 1, this);
            }
        }
    }

    void Update()
    {
        if (counterStarted)
        {
            if (counter < 0)
            {
                Die();
            }
            counter -= DeltaTime();
            
        }
    }

    // Appeler par l'animation Death
    public void Kill()
    {
        Destroy(gameObject);
    }

    protected override void Die()
    {
        base.Die();
        animations.SetBool("IsDead", true);
    }

    public void StartCountdown(float time)
    {
        counterStarted = true;
        counter = time;
    }
}
