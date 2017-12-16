using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Equipable
{
    [System.NonSerialized, FullSerializer.fsIgnore]
    public int originalAssetID;

    public abstract void Equip(int duplicateIndex);
    public abstract void Unequip();
}
