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
    
    protected Transform tr;

    protected virtual void Start()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        tr.position += speed * Time.deltaTime * (isAffectedByTimeScale ? timeScale : 1);
    }
}
