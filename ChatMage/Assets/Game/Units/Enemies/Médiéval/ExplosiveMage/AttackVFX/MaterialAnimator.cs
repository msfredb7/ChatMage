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
            var offset = material.mainTextureOffset;
            if (basedOnMotion)
            {
                offset += (tr.position - lastPosition).magnitude * offsetPerUnitOfMotion;
            }
            else
            {
                offset += offsetSpeed * Time.deltaTime;
            }
            offset.x = offset.x.Mod(1);
            offset.y = offset.y.Mod(1);
            material.mainTextureOffset = offset;
        }

        lastPosition = tr.position;
    }

    void OnDisable()
    {
        if (mutex != null && mutex.Value == this)
            mutex.Value = null;
    }
}
