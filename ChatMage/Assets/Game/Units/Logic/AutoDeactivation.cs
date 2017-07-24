using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivation : MonoBehaviour
{
    public new bool enabled = true;
    public float deactivationRange = 10;

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

        float delta = Mathf.Abs(cameraHeight - tr.position.y);

        if (delta > deactivationRange)
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
}
