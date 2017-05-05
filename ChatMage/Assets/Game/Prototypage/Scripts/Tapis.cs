using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapis : MonoBehaviour
{
    public float speed = 1;
    public float direction;
    public bool useBounds = true;
    public Vector2 bounds = new Vector2(10, 10);
    [Range(0, 1)]
    public float weight = 0.5f;

    private Transform tr;
    private Vector3 vSpeed;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {

        Vector3 vDir = WorldDirection() * speed;
        vSpeed = Vector3.Lerp(vSpeed, vDir, FixedLerp.Fix(weight));

        tr.position += vSpeed * Time.deltaTime;

        if (useBounds)
            tr.position = new Vector3(
                Mathf.Max(0, Mathf.Min(bounds.x, tr.position.x)),       //x
                Mathf.Max(0, Mathf.Min(bounds.y, tr.position.y)),       //y
                tr.position.z);                                         //z
    }



    Vector3 WorldDirection()
    {
        float rad = direction * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
    }
}
