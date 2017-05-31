using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Camera cam;
    public float Aspect { get { return cam.aspect; } }
    [Header("Settings")]
    public Vector2 defaultBounds;
    public float distance = -10;
    public bool followPlayer = false;

    private Vector2 screenSize;
    private Vector2 defaultToRealRatio;
    private Transform tr;
    private Transform player;

    void Awake()
    {
        tr = transform;
    }

    public void Init(Transform player)
    {
        this.player = player;

        //Screen bounds
        screenSize = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenSize.x, defaultBounds.y / screenSize.y);
    }

    public void CenterOnPlayer()
    {
        SetToHeight(player.position.y);
    }

    public float Height { get { return tr.position.y; } }

    public void SetToHeight(float height)
    {
        tr.position = new Vector3(0, height, distance);
    }

    void Update()
    {
        if (followPlayer && player != null)
            SetToHeight(player.position.y);
    }

    #region Bounds

    public Vector2 ScreenSize { get { return screenSize; } }

    public Vector3 AdjustVector(Vector3 position)
    {
        return new Vector3(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y, 0);
    }
    public Vector2 AdjustVector(Vector2 position)
    {
        return new Vector2(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y);
    }

    #endregion
}
