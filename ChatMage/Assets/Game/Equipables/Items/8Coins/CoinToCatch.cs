using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToCatch : MonoBehaviour {

    public SimpleEvent coinCatch;

    public SimpleColliderListener colliderListener;

    private bool alreadyCatch;

    void Start()
    {
        alreadyCatch = false;
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
    if(other.parentUnit.allegiance == Allegiance.Ally)
        {
            if (!alreadyCatch)
            {
                if (coinCatch == null)
                    Debug.LogWarning("Erreur dans Coin To Catch");
                alreadyCatch = true;
                coinCatch.Invoke();
                // TODO : Animation de coin pogner
                Destroy(gameObject);
            }
        }
    }
}
