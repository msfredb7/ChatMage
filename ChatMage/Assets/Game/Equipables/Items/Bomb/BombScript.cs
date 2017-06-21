using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : Vehicle {

    public SimpleColliderListener colliderListener;

    float explosionForce = 1;
    
    void Start()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    public void Explode()
    {
        // TODO
        Debug.Log("BOOM");
        Destroy(gameObject);
    }

    public void SetExplosionForce(float force)
    {
        explosionForce = force;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        // TODO
        Explode();
    }
}
