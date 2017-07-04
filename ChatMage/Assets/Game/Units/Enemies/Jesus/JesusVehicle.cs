using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusVehicle : EnemyVehicle {

    public JesusAnimator animator;

    public float cooldown = 5;
    private float counter;

    public override int Attacked(ColliderInfo on, int amount, Unit unit, ColliderInfo source = null)
    {
        if (amount <= 0)
            return 1;

        Die();
        return 0;
    }

    void Start()
    {
        cooldown = 1;
        counter = cooldown;
    }

    void Update()
    {
        if(counter < 0)
        {
            GoToLocation();
            counter = cooldown;
        }
        counter -= DeltaTime();
    }

    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }

    public void GoToLocation()
    {
        GotoPosition(GetRandomLocationAroundScreen());
    }

    Vector2 GetRandomLocationAroundScreen()
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(Game.instance.gameCamera.Right, Game.instance.gameCamera.Left), Position.y);

        return pos;
    }
}
