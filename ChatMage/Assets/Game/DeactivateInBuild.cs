using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateInBuild : MonoBehaviour
{
    void Awake()
    {
        if (!Application.isEditor)
        {
            gameObject.SetActive(false);
        }
    }
}
