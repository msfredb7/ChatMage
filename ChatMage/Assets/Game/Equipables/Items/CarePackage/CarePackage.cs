using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarePackage : MonoBehaviour {

    public List<GameObject> rewards = new List<GameObject>();

    public SimpleColliderListener colliderListener;

	// Use this for initialization
	void Start ()
    {
        colliderListener.onTriggerEnter += ColliderListener_onTriggerEnter;
    }

    private void ColliderListener_onTriggerEnter(ColliderInfo other, ColliderListener listener)
    {
        if(other.parentUnit.allegiance == Allegiance.Ally)
        {
            int rewardSelected = Random.Range(0, rewards.Count - 1);
            Instantiate(rewards[rewardSelected], transform.position, transform.rotation);

            // TODO: Animation d'explosion de la boite
            Destroy(gameObject);
        }
    }
}
