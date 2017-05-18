using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public abstract class Car : Equipable
{
    public abstract void OnInputUpdate(float horizontalInput);
}
