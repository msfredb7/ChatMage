using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public class Unit_Event : UnityEvent<Unit> { }
    public Unit_Event onDestroy = new Unit_Event();
    public float timeScale = 1;
}
