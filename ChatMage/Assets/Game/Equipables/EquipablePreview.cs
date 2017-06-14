using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EquipablePreview : BaseScriptableObject
{
    public const string SAVE_PREFIX = "equip";

    public string displayName;
    public string description;
    public string effects;
    public string equipableAssetName;
    public Sprite icon;
    public EquipableType type;
    public bool unlocked = false;


    //[InspectorHeader("Ne pas utiliser")]
    //public bool affectSmash;
    //public bool specialInput;
    //public string specialInputTooltipText;

    [InspectorHeader("Peut-être")]
    public Sprite largeIcon;
    public AudioClip selectSound;

    public void Save()
    {
        string equipableKey = SAVE_PREFIX + equipableAssetName;
        GameSaves.instance.SetBool(GameSaves.Type.Armory, equipableKey, unlocked);
    }

    public void Load()
    {
        string equipableKey = SAVE_PREFIX + equipableAssetName;
        if (GameSaves.instance.ContainsBool(GameSaves.Type.Armory, equipableKey))
            unlocked = GameSaves.instance.GetBool(GameSaves.Type.Armory, equipableKey);
        else
        {
            GameSaves.instance.SetBool(GameSaves.Type.Armory, equipableKey, unlocked);
            GameSaves.instance.SaveData(GameSaves.Type.Armory);
        }
    }
}
