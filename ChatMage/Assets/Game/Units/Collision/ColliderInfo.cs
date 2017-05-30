using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInfo : MonoBehaviour
{
    public Unit parentUnit;
    public GameObject groupParent;
    public GameObject GroupParent { get { return groupParent == null ? gameObject : groupParent; } }
}
