using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusVehicle : EnemyVehicle {

    public JesusBrain brain;
    public JesusAnimator animator;
    public MultipleColliderListener colliderListener;

    public int startingHealth = 5;
    public float forceToRektPlayer = 5f;
    public bool canBeHit = true;

    private int health;

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
        animator.DisplayHealthBar();
        health = startingHealth;
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
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
        animator.UpdateHealthBar(health,startingHealth);
        return 1;
    }

    protected override void Die()
    {
        base.Die();

        // ANIMATION DE DESTRUCTIONS DU BOSS
        animator.JesusDeath();

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
