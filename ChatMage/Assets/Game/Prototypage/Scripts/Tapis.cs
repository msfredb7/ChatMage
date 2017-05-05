using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapis : MonoBehaviour
{
    public float speed = 1;
    public float direction;
    public bool useBounds = true;
    public Vector2 bounds = new Vector2(10, 10);
    [Range(0, 1)]
    public float weight = 0.1f;
    public Text weightText;
    public Vector3 CurrentVelocity
    {
        get { return vSpeed; }
    }

    private Transform tr;
    private Vector3 vSpeed;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {

        Vector3 vDir = WorldDirection() * speed;
        vSpeed = Vector3.Lerp(vSpeed, vDir, FixedLerp.Fix(weight >= 1f ? 1 : weight/10));

        Vector3 wasPos = tr.position;
        tr.position += vSpeed * Time.deltaTime;

        if (useBounds)
            tr.position = new Vector3(
                Mathf.Max(0, Mathf.Min(bounds.x, tr.position.x)),       //x
                Mathf.Max(0, Mathf.Min(bounds.y, tr.position.y)),       //y
                tr.position.z);                                         //z

        vSpeed = (tr.position - wasPos) / Time.deltaTime;
    }



    public Vector3 WorldDirection()
    {
        float rad = direction * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
    }

    public void SetWeight(float weight)
    {
        this.weight = weight;
        weightText.text = "Weight: " + ((float)((int)(weight * 100))) / 100;
    }
}
