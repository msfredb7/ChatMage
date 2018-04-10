using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController controller;

    public Locker Enabled = new Locker();
    public Locker CanSmash = new Locker();
    public Locker CanTurn = new Locker();

    private int turning;
    private bool smashPress;

    void Start()
    {
        ClearFlags();
    }

    public void Init(PlayerController controller)
    {
        this.controller = controller;
        if (controller == null)
            Debug.LogError("PlayerController is null");
    }

    void Update()
    {
        if (controller == null || !Game.Instance.gameStarted || !Enabled)
            return;


        //Sur mobile ou non ?
        if (Application.isMobilePlatform)
        {
            //On check tout les touchs
            for (int i = 0; i < Input.touchCount; i++)
            {
                OnPlayerTouch(Input.GetTouch(i).position);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                //Click de souris
                Vector2 pos = Input.mousePosition;
                OnPlayerTouch(pos);
            }
            else
            {
                //Touch de clavier
                if (HorizontalAxis > 0.1f)
                {
                    if (CanTurn)
                        turning++;
                }

                if (HorizontalAxis < -0.1f)
                {
                    if (CanTurn)
                        turning--;
                }

                if (Game.Instance.gameRunning && UltimateButtonDown)
                {
                    if (CanSmash)
                        smashPress = true;
                }
            }
        }

        ApplyFlags();
        ClearFlags();
    }

    float HorizontalAxis
    {
        get
        {
            //var keyboard = Input.GetAxisRaw("Turn (Keys)");
            //var joystick = Input.GetAxisRaw("Turn (Joystick)");
            //if (keyboard.Abs() > joystick.Abs())
            //    return keyboard;
            //else
            //    return joystick;

            return Input.GetAxisRaw("Turn");
        }
    }

    bool UltimateButtonDown
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space) ||
               Input.GetButtonDown("Activate Power (1)") ||
               Input.GetButtonDown("Activate Power (2)") ||
               Input.GetButtonDown("Activate Power (3)") ||
               Input.GetButtonDown("Activate Power (4)");
        }
    }

    void OnPlayerTouch(Vector2 pixelPosition)
    {
        if (!Application.isMobilePlatform || !Game.Instance.gameRunning)
            return;

        float x = pixelPosition.x;
        float regionWidth = Screen.width / 3f;

        if (x < regionWidth)
        {
            if (CanTurn)
                turning--;
        }
        else if (x < regionWidth * 2)
        {
            if (CanSmash)
                smashPress = true;
        }
        else if (x < regionWidth * 3)
        {
            if (CanTurn)
                turning++;
        }
    }

    void ApplyFlags()
    {
        if (turning > 0)
            controller.playerDriver.TurnRight();
        else if (turning < 0)
            controller.playerDriver.TurnLeft();

        if (smashPress)
            controller.playerSmash.SmashClick();
    }
    void ClearFlags()
    {
        turning = 0;
        smashPress = false;
    }
}
