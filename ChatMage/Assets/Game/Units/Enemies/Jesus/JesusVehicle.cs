using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusVehicle : EnemyVehicle {

    public JesusAnimator animator;

    public int health = 5;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0)
            return 1;

        if(unit is JesusRock)
            Damaged();

        return 0;
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void Damaged()
    {
        health--;
        if (health <= 0)
            Die();
    }

    protected override void Die()
    {
        base.Die();

        // ANIMATION DE DESTRUCTIONS DU BOSS

        Destroy(gameObject);
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
