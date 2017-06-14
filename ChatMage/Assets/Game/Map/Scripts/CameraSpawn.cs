using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawn : MonoBehaviour
{
    public float Height { get { return transform.position.y; } }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
        Gizmos.DrawCube(transform.position, new Vector3(16,9, 1));
    }
}
