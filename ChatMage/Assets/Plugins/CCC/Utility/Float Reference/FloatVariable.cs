using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FV_NewFloat")]
public class FloatVariable : ScriptableObject
{
    public float Value;

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public static implicit operator float(FloatVariable reference)
    {
        return reference.Value;
    }

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(float amount)
    {
        Value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        Value += amount.Value;
    }
}
