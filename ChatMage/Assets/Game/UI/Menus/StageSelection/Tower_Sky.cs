using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Sky : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform mountain;
    [SerializeField] TriColored triColored;

    [Header("Settings")]
    [SerializeField] float scalePerUnit = 0.1f;
    [SerializeField] float screenTop = 5.1f;
    [SerializeField] float screenBottom = -5.1f;
    [SerializeField] Gradient topGradient;
    [SerializeField] Gradient bottomGradient;
    [SerializeField] float gradientSpeed = -0.1f;

    private Transform tr;
    private Vector3 stdScale;
    private float mountainOffset;
    private CCC.Math.NeverReachingCurve colorCurve;

    void Awake()
    {
        tr = transform;
        stdScale = tr.localScale;
        mountainOffset = GetBottomPosition() - mountain.position.y;
        UpdateHeight();
        Object.FindObjectsOfType<GameObject>();
        colorCurve = new CCC.Math.NeverReachingCurve(0, 1, gradientSpeed, mountain.position.y);
    }

    void Update()
    {
        UpdateHeight();
        UpdateColor();
    }

    void UpdateHeight()
    {
        SetBottomAt(mountain.position.y + mountainOffset);
    }

    void UpdateColor()
    {
        var curveValue = colorCurve.Evalutate(mountain.position.y);
        triColored.ColorR = bottomGradient.Evaluate(curveValue);
        triColored.ColorG = topGradient.Evaluate(curveValue);
    }

    public void SetBottomAt(float height)
    {
        var size = screenTop - height;
        size = Mathf.Clamp(size, 0, screenTop - screenBottom);
        tr.position = new Vector3(0, screenTop - (size / 2), 0);
        tr.localScale = new Vector3(stdScale.x, size * scalePerUnit, 1);
    }

    float GetBottomPosition()
    {
        return tr.position.y - (tr.localScale.y / scalePerUnit) / 2;
    }
}
