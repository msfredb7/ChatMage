using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public enum ControlMode { Rigid, Smooth }
    public Tapis tapis;
    public Transform visuals;
    public Camera cam;
    public Ball ballPrefab;
    [Header("Controllers")]
    public PointerListener hor_LeftButton;
    public PointerListener hor_MiddleButton;
    public PointerListener hor_RightButton;
    public PointerListener vert_LeftButton;
    public PointerListener vert_MiddleButton;
    public PointerListener vert_RightButton;
    public PointerListener vertSlider_MiddleButton;
    public Slider vertSlider_Slider;
    public Text turnAccText;
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
        tapis.bounds = Game.instance.ScreenBounds;
        hor_MiddleButton.onClick.AddListener(OnMiddleClick);
        vert_MiddleButton.onClick.AddListener(OnMiddleClick);
        vertSlider_MiddleButton.onClick.AddListener(OnMiddleClick);
    }

    void OnMiddleClick()
    {
        GameObject ball = Instantiate(ballPrefab.gameObject);
        ball.transform.position = transform.position;
        ball.GetComponent<Rigidbody2D>().velocity = tapis.CurrentVelocity*0.5f + tapis.WorldDirection() *tapis.speed*1.75f;
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
        float horizontal = 0;

        //Slider
        if(vertSlider_Slider.gameObject.activeInHierarchy)
        {
            if (vertSlider_Slider.GetComponent<PointerListener>().isIn)
            {
                horizontal = vertSlider_Slider.value;
                if (horizontal > 1)
                    horizontal = 1;
                else if (horizontal < -1)
                    horizontal = -1;
            }
            else
                horizontal = 0;
        }

        //Keys
        if (Input.GetKey(KeyCode.LeftArrow) || hor_LeftButton.isIn || vert_LeftButton.isIn)
            horizontal -= 1;
        if (Input.GetKey(KeyCode.RightArrow) || hor_RightButton.isIn || vert_RightButton.isIn)
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

    public void SetTurnAcceleration(float value)
    {
        turnAcceleration = value;
        turnAccText.text = "Turn Acceleration: " + ((float)((int)(value * 100))) / 100;
    }

}
