using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEnemy : MonoBehaviour {

    public float speed;

    void Update()
    {
        transform.right = Game.instance.Player.transform.position - transform.position;
        GetComponent<MovingUnit>().speed = transform.right.normalized * speed;
    }
}
