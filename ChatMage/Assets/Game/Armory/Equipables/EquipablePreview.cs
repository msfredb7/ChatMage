using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EquipablePreview : BaseScriptableObject
{
    public const string SAVE_PREFIX = "equip";

    //Temporaire. Lorsqu'on veut l'enlever, simplement delete �a et les lignes:
    //  LoadoutElementInspector.cs ligne 101, 103 et 104
    public bool canBeCheatUnlocked = false;

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
        string equipableKey = SAVE_PREFIX + name;
        DataSaver.instance.SetBool(DataSaver.Type.Armory, equipableKey, unlocked);
    }

    public void Load()
    {
        unlocked = IsUnlocked(name);
    }

    public static bool IsUnlocked(string previewName)
    {
        string equipableKey = SAVE_PREFIX + previewName;

        if (DataSaver.instance.ContainsBool(DataSaver.Type.Armory, equipableKey))
            return DataSaver.instance.GetBool(DataSaver.Type.Armory, equipableKey);
        else
            return DEFAULT_UNLOCKS.Contains(previewName);
    }

    // HARD CODE TEMPORAIRE
    static readonly string[] DEFAULT_UNLOCKS =
        {
            "CarPrv_Vadrouille",
            "SmPrv_Warudo"
        };
}
