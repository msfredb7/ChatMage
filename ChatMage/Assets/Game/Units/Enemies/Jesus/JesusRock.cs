using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusRock : Unit {

    public SimpleColliderListener colliderListener;
    public float rangeToTake = 3f;
    public float speedMinToHit = 10f;

    private bool canHit;

	void Start ()
    {
        canHit = false;
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
    }

    void Update()
    {
        // Si la velocidad est grande
        if (Speed.x > speedMinToHit && Speed.y > speedMinToHit)
            canHit = true;
        else
            canHit = false;
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        if(canHit && other is IAttackable)
            other.GetComponent<IAttackable>().Attacked(other, 1, this);
    }

    public bool TakeTheRock(Vector2 takerPosition)
    {
        if (Vector2.Distance(takerPosition, transform.position) < rangeToTake)
        {
            // The Rock has been taken !
            Destroy(gameObject);
            return true;
        }
        else
            return false;
    }
}
