using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VarVariable<T> : ScriptableObject
{
    public T Value;

    public static implicit operator T(VarVariable<T> reference)
    {
        return reference.Value;
    }
}