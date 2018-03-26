using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VarVariable<T> : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    private T RuntimeValue;
    [SerializeField] private T value;

    public T Value
    {
        get { return RuntimeValue; }
        set { RuntimeValue = value; }
    }

    public void OnEnable()
    {
        RuntimeValue = value;
        Debug.Log(name + ":" + value);
    }

    public void OnBeforeSerialize()
    {

    }

    public static implicit operator T(VarVariable<T> reference)
    {
        return reference.RuntimeValue;
    }
}