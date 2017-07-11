using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JesusRock : Unit
{
    public SimpleColliderListener colliderListener;

    public float rangeToTake = 3f;
    public float distanceUntilStopped = 5;
    public float rockSpeed = 5f;
    public float cooldownUntilSolid = 1f;

    public SimpleEvent onRockTaken;

    private Vector2 startingPosition;
    private float countdown;

    [HideInInspector]
    public bool canHit;

    void Start()
    {
        // La roche peut pas faire mal quand on vient de la lancer
        canHit = true;
        startingPosition = transform.position; // on start le systeme de lancement/atterisage
        colliderListener.onCollisionEnter += ColliderListener_onCollisionEnter; // on doit savoir si le roche hit dequoi
        Speed = Speed.normalized * rockSpeed;
        gameObject.layer = Layers.NO_COLLISION; // Faut pas que quand la roche spawn elle soit prise sous le joueur
        countdown = cooldownUntilSolid;
    }

    protected override void Update()
    {
        // Si on a parcouru une certaine distance
        if(Vector2.Distance(startingPosition,transform.position) > distanceUntilStopped)
        {
            // on arrete et on attend pour repartir
            Speed *= 0;
            startingPosition = transform.position;
            canHit = false;
        }

        if (countdown < 0)
            gameObject.layer = Layers.SOLID_ENEMIES;
        else
            countdown -= DeltaTime();
    }

    private void ColliderListener_onCollisionEnter(ColliderInfo other, Collision2D collision, ColliderListener listener)
    {
        // Si le joueur a frapper la roche
        if (other.parentUnit is PlayerVehicle)
        {
            Speed = Speed.normalized * rockSpeed;
            startingPosition = transform.position; // et on reset le systeme de lancement/atterisage
        }

        // Si on a hit quelque chose qui peut etre endommager
        IAttackable unitAttackable = other.parentUnit.GetComponent<IAttackable>();
        // et que la roche peut hit
        if (canHit && unitAttackable != null)
        {
            other.parentUnit.GetComponent<IAttackable>().Attacked(other, 1, this);
        }
    }

    // Quelqu'un essaie de prendre la roche !
    public bool TakeTheRock(Vector2 takerPosition)
    {
        // Si la roche est proche
        if (Vector2.Distance(takerPosition, transform.position) < rangeToTake)
        {
            // The Rock has been taken !
            onRockTaken.Invoke();
            Destroy();
            return true;
        }
        else
            return false;
    }
}
