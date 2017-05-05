using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PublicSingleton<Game>
{
    public Camera cam;

    public float Aspect { get { return cam.aspect; } }
    public Vector2 ScreenBounds { get { return screenBounds; } }
    private Vector2 screenBounds;

    protected override void Awake()
    {
        base.Awake();
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
    }
}
