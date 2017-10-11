using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public enum Type { CircularEmit, CircularReceive, LinearReceive }
    public Sprite circularEmit;
    public Sprite circularReceive;
    public Sprite linearReceive;

    public void SetType(Type type)
    {

    }

    public void SetTarget(Unit unit, bool willMove)
    {

    }

    public void SetTarget(Transform tr, bool willMove)
    {

    }
}
