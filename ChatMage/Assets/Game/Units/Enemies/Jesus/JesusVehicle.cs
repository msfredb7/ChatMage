using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusVehicle : EnemyVehicle {

    public JesusBrain brain;
    public JesusAnimator animator;
    public MultipleColliderListener colliderListener;

    public int health = 5;
    public float forceToRektPlayer = 5f;
    public bool canBeHit = true;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0)
            return 1;

        if (brain.damagableWhilePickingRock)
        {
            if(!brain.pickingRockAnimationOver)
                return Damaged();
            return 1;
        } else
        {
            return Damaged();
        }
    }

    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
    }

    void Update()
    {
    }

    private int Damaged()
    {
        if (!canBeHit)
            return 1;
        health--;
        if (health <= 0)
        {
            Die();
            return 0;
        }
        animator.OnHitFlashAnimation();
        return 1;
    }

    protected override void Die()
    {
        base.Die();

        // ANIMATION DE DESTRUCTIONS DU BOSS

        Destroy();
    }

    public void GoToLocation(Vector2 location)
    {
        // ANIMATION DE DÉPLACEMENT
        GotoPosition(location);
    }

    Vector2 GetRandomLocationAroundScreen()
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(Game.instance.gameCamera.Right, Game.instance.gameCamera.Left), Position.y);

        return pos;
    }
}
