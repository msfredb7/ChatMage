using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Equipable
{
    public Sprite ingameIcon;

    [System.NonSerialized, FullSerializer.fsIgnore]
    public int originalAssetID;

    public abstract void Equip(int duplicateIndex);
    public abstract void Unequip();
    public override void OnGameReady() { }
    public override void OnGameStarted() { }
    public override void OnUpdate() { }
}
