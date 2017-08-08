using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawn : MonoBehaviour
{
    public float Height { get { return transform.position.y; } }

    void OnDrawGizmos()
    {
        Gizmos.color = (Color.white).ChangedAlpha(0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(GameCamera.DEFAULT_SCREEN_WIDTH, GameCamera.DEFAULT_SCREEN_HEIGHT, 1));

        DebugExtension.DrawBounds(
            new Bounds(transform.position, new Vector3(GameCamera.DEFAULT_SCREEN_WIDTH, GameCamera.DEFAULT_SCREEN_HEIGHT, 1)),
            Gizmos.color.ChangedAlpha(1));
    }
}
