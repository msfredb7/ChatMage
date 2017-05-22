using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController controller;
    public PointerListener leftButton;
    public PointerListener middleButton;
    public PointerListener rightButton;

    void Start()
    {
        middleButton.onClick.AddListener(OnMiddleClick);
    }

    public void Init(PlayerController controller)
    {
        this.controller = controller;
        if (controller == null)
            Debug.LogError("PlayerController is null");
    }

    void OnMiddleClick()
    {
        controller.playerSmash.SmashClick();
    }

    void Update()
    {
        if (controller == null)
            return;

        if (rightButton.isIn || Input.GetKey(KeyCode.RightArrow))
            controller.playerDriver.TurnRight();
        if (leftButton.isIn || Input.GetKey(KeyCode.LeftArrow))
            controller.playerDriver.TurnLeft();
        if (Input.GetKey(KeyCode.Space))
            OnMiddleClick();
    }
}
