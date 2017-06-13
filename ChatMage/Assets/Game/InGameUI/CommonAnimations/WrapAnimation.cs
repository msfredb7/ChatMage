using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WrapAnimation : MonoBehaviour
{
    bool wrapOver;

    public virtual void Init()
    {
        wrapOver = false;
    }

    public virtual void End()
    {
        wrapOver = true;
    }

    public virtual bool IsComplete()
    {
        return wrapOver;
    }
}
