using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LoadoutEquipable
{
    public EquipablePreview preview;
    public delegate void Event(LoadoutEquipable e);

    public event Event onEquip;
    public event Event onUnequip;

    private bool isEquipped = false;
    public bool IsEquipped { get { return isEquipped; } }

    public LoadoutEquipable(EquipablePreview preview)
    {
        this.preview = preview;
    }

    public bool IsUnlocked { get { return preview.unlocked; } }

    /// <summary>
    /// Ne pas appeler ceci hors de la classe Loadout
    /// </summary>
    public void OnEquip()
    {
        isEquipped = true;
        if (onEquip != null)
            onEquip(this);
    }
    /// <summary>
    /// Ne pas appeler ceci hors de la classe Loadout
    /// </summary>
    public void OnUnequip()
    {
        isEquipped = false;
        if (onUnequip != null)
            onUnequip(this);
    }
}
