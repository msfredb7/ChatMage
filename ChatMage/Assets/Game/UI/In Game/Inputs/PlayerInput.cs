using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController controller;

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
        if (controller == null || !Game.instance.gameStarted)
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
                if (Input.GetKey(KeyCode.RightArrow))
                    turning ++;
                if (Input.GetKey(KeyCode.LeftArrow))
                    turning--;
                if (Input.GetKeyDown(KeyCode.Space))
                    smashPress = true;
            }
        }

        ApplyFlags();
        ClearFlags();
    }

    void OnPlayerTouch(Vector2 pixelPosition)
    {
        float x = pixelPosition.x;
        float regionWidth = Screen.width / 3f;

        if (x < regionWidth)
            turning--;
        else if (x < regionWidth * 2)
            smashPress = true;
        else if (x < regionWidth * 3)
            turning++;
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
