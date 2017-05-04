using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapis : MonoBehaviour
{
    public float currentSpeed;
    public float currentDirection;

    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.position += WorldDirection() * currentSpeed * Time.deltaTime;
    }

    Vector3 WorldDirection()
    {
        return new Vector3(Mathf.Cos(currentDirection), Mathf.Sin(currentDirection), 0);
    }
}
