using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class EquipablePreview : BaseScriptableObject
{
    public string displayName;
    public string description;
    public string effects;
    public string equipableAssetName;
    public Sprite icon;
    public EquipableType type;
    public bool unlocked = false;
    public bool affectSmash;
    public bool specialInput;
    public string specialInputTooltipText;

    [InspectorHeader("Peut-être")]
    public Sprite largeIcon;
    public AudioClip selectSound;
}
