using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public enum ControlMode { Rigid, Smooth }
    public Tapis tapis;
    public Transform visuals;
    public Camera cam;
    [Header("Settings")]
    public ControlMode mode;
    public bool slowsOnTurns = false;
    public float slowedMultiplier = 0.5f;
    public float turnSpeed;
    public float acceleration;
    [Header("Smooth")]
    public float turnAcceleration = 3;
    public float turnClutch = 0.25f;

    //private float turnSpeedMultiplier;

    private float lastHorizontal = 0;
    private float lastVertical = 0;
    private float originalSpeed;

    void Start()
    {
        originalSpeed = tapis.speed;
        tapis.bounds = new Vector2(cam.aspect * cam.orthographicSize * 2, cam.orthographicSize * 2);
    }

    void Update()
    {
        visuals.rotation = Quaternion.Euler(0, 0, tapis.direction);
        //
        float horizontal = 0;
        float vertical = 0;
        switch (mode)
        {
            case ControlMode.Rigid:
                horizontal = GetRigidAddHorizontal();
                vertical = GetRigidAddVertical();
                break;
            case ControlMode.Smooth:
                float newhorizontal = GetRigidAddHorizontal();
                if (newhorizontal == 0)
                    horizontal = 0;
                else
                {
                    if ((newhorizontal < 0 && lastHorizontal > 0) || (newhorizontal > 0 && lastHorizontal < 0))
                        lastHorizontal = 0;
                    if (Mathf.Abs(newhorizontal) > turnClutch && Mathf.Abs(lastHorizontal) < turnClutch)
                        lastHorizontal = newhorizontal * turnClutch;
                    horizontal = Mathf.MoveTowards(lastHorizontal, newhorizontal, Time.deltaTime * turnAcceleration);
                }
                break;
        }

        if (slowsOnTurns)
        {
            if (horizontal != 0)
            {
                tapis.speed = Mathf.MoveTowards(tapis.speed, originalSpeed * slowedMultiplier, acceleration * Time.deltaTime);
            }
            else
            {
                tapis.speed = Mathf.MoveTowards(tapis.speed, originalSpeed, acceleration * Time.deltaTime);
            }
        }

        tapis.direction += -horizontal * turnSpeed * Time.deltaTime;
        tapis.speed = Mathf.Max(0, tapis.speed + (acceleration * vertical * acceleration * Time.deltaTime));

        lastHorizontal = horizontal;
        lastVertical = vertical;
    }

    float GetRigidAddHorizontal()
    {
        int horizontal = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontal -= 1;
        if (Input.GetKey(KeyCode.RightArrow))
            horizontal += 1;
        return horizontal;
    }

    float GetRigidAddVertical()
    {
        int vertical = 0;
        if (Input.GetKey(KeyCode.DownArrow))
            vertical -= 1;
        if (Input.GetKey(KeyCode.UpArrow))
            vertical += 1;
        return vertical;
    }

}
