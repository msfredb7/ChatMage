using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EquipablePreview : BaseScriptableObject
{
    public const string SAVE_PREFIX = "equip";

    public string displayName;
    public string effects;
    public string equipableAssetName;
    public Sprite icon;
    public EquipableType type;
    public bool Unlocked { get { return unlocked; } }
    private bool unlocked = false;


    public void MarkAsUnlocked()
    {
        unlocked = true;
        string equipableKey = SAVE_PREFIX + equipableAssetName;
        GameSaves.instance.SetBool(GameSaves.Type.Armory, equipableKey, unlocked);
    }

    public void Load()
    {
        unlocked = IsUnlocked(equipableAssetName);
    }

    public static bool IsUnlocked(string assetName)
    {
        string equipableKey = SAVE_PREFIX + assetName;

        if (GameSaves.instance.ContainsBool(GameSaves.Type.Armory, equipableKey))
            return GameSaves.instance.GetBool(GameSaves.Type.Armory, equipableKey);
        else
            return true;
    }
}
