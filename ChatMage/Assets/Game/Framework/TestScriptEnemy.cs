using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEnemy : MonoBehaviour {

    public float speed;

    void FixedUpdate()
    {
        var dir = Game.instance.Player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GetComponent<MovingUnit>().Speed = transform.rotation.eulerAngles.normalized * speed;
    }
}
