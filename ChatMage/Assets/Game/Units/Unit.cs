using CCC.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public class Unit_Event : UnityEvent<Unit> { }
    public Unit_Event onDestroy = new Unit_Event();
    public float timeScale = 1;
    public Locker isAffectedByTimeScale = new Locker();


    public virtual Vector3 Speed()
    {
        return Vector3.zero;
    }
    public virtual Vector3 WorldDirection()
    {
        return Vector3.up;
    }
    public virtual Vector2 WorldDirection2D()
    {
        return Vector2.up;
    }
    public float DeltaTime()
    {
        return isAffectedByTimeScale ? Time.deltaTime * timeScale : Time.deltaTime;
    }

    void OnDestroy()
    {
        onDestroy.Invoke(this);
    }
}
