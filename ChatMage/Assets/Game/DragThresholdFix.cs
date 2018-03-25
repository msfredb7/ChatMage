using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragThresholdFix : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] float referenceHeight = 1080;

    void Start()
    {
        eventSystem.pixelDragThreshold *= Mathf.RoundToInt(Screen.height / referenceHeight);
    }
}
