using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(VectorShaker)), ExecuteInEditMode]
public class ShakeTransform : MonoBehaviour
{
    public VectorShaker shaker;

    [HideInInspector]
    public Vector2 anchor;

    private Vector2 lastAnchor;

    private bool rectt = false;
    private Transform tr;

    void Awake()
    {
        shaker = GetComponent<VectorShaker>();
        FetchAnchor();
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (rectt)
            {
                anchor += (tr as RectTransform).anchoredPosition - lastAnchor;
                (tr as RectTransform).anchoredPosition = anchor + shaker.CurrentVector;
            }
            else
            {
                anchor += (Vector2)tr.localPosition - lastAnchor;
                tr.localPosition = anchor + shaker.CurrentVector;
            }
            lastAnchor = anchor;
        }


    }

    public void FetchAnchor()
    {
        tr = transform;
        rectt = tr is RectTransform;

        if (rectt)
        {
            anchor = (tr as RectTransform).anchoredPosition;
        }
        else
        {
            anchor = tr.localPosition;
        }
        lastAnchor = anchor;
    }
}
