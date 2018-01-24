using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheatButton : MonoBehaviour
{
    void OnEnable()
    {
        if (!Debug.isDebugBuild)
            gameObject.SetActive(false);
    }

    public abstract void Execute();
}
