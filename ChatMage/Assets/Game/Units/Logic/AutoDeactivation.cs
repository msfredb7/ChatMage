using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivation : MonoBehaviour
{
    public new bool enabled = true;
    public const float DFLT_DEACT_RANGE = 7.5f; // Cetait 6 avant

    public bool checkWhenUnderCamera = true;
    public bool checkWhenAboveCamera = true;
    public bool overrideDefaultDeactivationRange = false;
    public float newDeactivationRange = 10;

    [System.NonSerialized]
    private Transform tr;
    [System.NonSerialized]
    private GameCamera cam;

    [System.NonSerialized]
    public LinkedListNode<AutoDeactivation> gameNode;

    void Awake()
    {
        tr = transform;

        Unit myUnit = GetComponent<Unit>();
        if(myUnit != null)
            myUnit.onDeath += MyUnit_onDeath;
    }

    private void MyUnit_onDeath(Unit unit)
    {
        enabled = false;
    }

    public void CheckActivation(float cameraHeight)
    {
        if (!enabled)
            return;

        float delta = tr.position.y - cameraHeight;

        if(delta > 0)
        {
            if (checkWhenAboveCamera)
                CheckABSActivation(delta);
        }
        else
        {
            if (checkWhenUnderCamera)
                CheckABSActivation(-delta);
        }
    }

    public void CheckABSActivation(float delta)
    {
        if (delta > GetDeactivationRange())
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
        else
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }
    }

    private float GetDeactivationRange()
    {
        return overrideDefaultDeactivationRange ? newDeactivationRange : DFLT_DEACT_RANGE;
    }
}
