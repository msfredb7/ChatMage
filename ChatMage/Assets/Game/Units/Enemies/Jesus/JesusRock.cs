using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusRock : Unit
{
    public SimpleColliderListener colliderListener;
    public float rangeToTake = 3f;
    public float speedMinToHit = 10f;

    public SimpleEvent onRockTaken;

    private bool canHit;

    void Start()
    {
        canHit = true;
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter;
    }

    void Update()
    {
        // Si la velocidad est grande
        if (Speed.magnitude > speedMinToHit)
            canHit = true;
        else
            canHit = false;
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        IAttackable unitAttackable = other.parentUnit.GetComponent<IAttackable>();
        if (canHit && unitAttackable != null)
            other.parentUnit.GetComponent<IAttackable>().Attacked(other, 1, this);
    }

    public bool TakeTheRock(Vector2 takerPosition)
    {
        // Si la boule va tellement vite qu'elle peut frapper, tu ne peux pas la prendre
        if (canHit)
            return false;
        if (Vector2.Distance(takerPosition, transform.position) < rangeToTake)
        {
            // The Rock has been taken !
            onRockTaken.Invoke();
            Destroy(gameObject);
            return true;
        }
        else
            return false;
    }
}
