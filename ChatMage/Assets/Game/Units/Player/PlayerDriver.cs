using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDriver
{
    public PlayerController player;

    public PlayerDriver(PlayerController player)
    {
        this.player = player;
    }

    public abstract void Update(float horizontalInput);
}
