using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEnemy : MonoBehaviour {

    public float speed;

    void Update()
    {
        var dir = Game.instance.Player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GetComponent<MovingUnit>().speed = transform.rotation.eulerAngles.normalized * speed;
    }
}
