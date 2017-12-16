using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Equipable
{
    public abstract void Equip();
    public abstract void Unequip();
}
