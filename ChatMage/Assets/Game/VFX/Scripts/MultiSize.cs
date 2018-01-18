using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSize : MonoBehaviour
{
    [SerializeField] private float smallSize = 0.5f;
    [SerializeField] private float mediumSize = 0.75f;
    [SerializeField] private float largeSize = 1;

    public enum Size { Small, Medium, Large }

    private Transform tr;

    void Awake()
    {
        tr = transform;
    }

    public void SetSize(Size size)
    {
        float scale = 1;
        switch (size)
        {
            case Size.Small:
                scale = smallSize;
                break;
            case Size.Medium:
                scale = mediumSize;
                break;
            case Size.Large:
                scale = largeSize;
                break;
        }

        Tr.localScale = Vector3.one * scale;
    }

    private Transform Tr
    {
        get
        {
            if (tr == null)
                tr = transform;
            return tr;
        }
    }
}
