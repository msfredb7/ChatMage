using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_SpeedRecorder : MonoBehaviour
{
    public float Velocity { get; private set; }
    public float WorldVelocity { get { return Velocity01 * cam.orthographicSize * 2; } }
    public float Velocity01 { get { return Velocity / Screen.height; } }
    [SerializeField] Camera cam;

    private float wasPos;
    RectTransform tr;
    private bool skipFrame = true;

    private void Awake()
    {
        tr = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var newPos = tr.position.y;

        if (!skipFrame)
            Velocity = (newPos - wasPos) / Time.deltaTime;

        wasPos = newPos;
        skipFrame = false;
    }
}
