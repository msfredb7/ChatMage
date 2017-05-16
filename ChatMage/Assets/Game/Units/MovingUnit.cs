using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Utility;

public class MovingUnit : Unit
{
    [System.NonSerialized]
    public Vector3 speed;
    public Locker canMove = new Locker();
    public Locker isAffectedByTimeScale = new Locker();

    private Transform tr;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.position += speed * Time.deltaTime * (isAffectedByTimeScale ? timeScale : 1);
    }
}
