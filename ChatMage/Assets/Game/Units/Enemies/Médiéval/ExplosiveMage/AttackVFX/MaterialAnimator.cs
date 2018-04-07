using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public Material material;
    public bool basedOnMotion = false;

    [HideIf("basedOnMotion")]
    public Vector2 offsetSpeed;
    [ShowIf("basedOnMotion")]
    public Vector2 offsetPerUnitOfMotion;

    public UnityObjectVariable mutex;

    private Vector3 lastPosition;
    private Transform tr;

    void Awake()
    {
        tr = transform;
    }

    void Update()
    {
        if (mutex != null && mutex.Value == null)
            mutex.Value = this;

        if (mutex == null || mutex.Value == this)
        {
            if (basedOnMotion)
            {
                material.mainTextureOffset += (tr.position - lastPosition).magnitude * offsetPerUnitOfMotion;
            }
            else
            {
                material.mainTextureOffset += offsetSpeed * Time.deltaTime;
            }
        }

        lastPosition = tr.position;
    }

    void OnDisable()
    {
        if (mutex != null && mutex.Value == this)
            mutex.Value = null;
    }
}
