using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerController controller;

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            controller.TurnRight();
        if (Input.GetKey(KeyCode.LeftArrow))
            controller.TurnLeft();
    }
}
