using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionManager : MonoBehaviour {

	public void OnHit()
    {
        Destroy(gameObject);
    }
}
