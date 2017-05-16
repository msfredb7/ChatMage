using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vehicle vehicle;
    public Transform body;

    [System.NonSerialized]
    private PlayerDriver driver;
    private float horizontalInput;

    private void Start()
    {
        //Temporaire
        driver = new DemoDriver(this);
    }

    private void Update()
    {
        if (driver != null)
            driver.Update(horizontalInput);

        horizontalInput = 0;
    }

    public void TurnLeft()
    {
        horizontalInput -= 1;
    }

    public void TurnRight()
    {
        horizontalInput += 1;
    }
}
