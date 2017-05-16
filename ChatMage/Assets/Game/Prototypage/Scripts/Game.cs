using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : PublicSingleton<Game>
{
    public Camera cam;

    public float Aspect { get { return cam.aspect; } }
    public Vector2 ScreenBounds { get { return screenBounds; } }
    public Vector2 defaultBounds;
    private Vector2 screenBounds;
    private Vector2 defaultToRealRatio;

    public UnityEvent onGameReady = new UnityEvent();
    public UnityEvent onGameStarted = new UnityEvent();

    public List<GameObject> units = new List<GameObject>();

    public void Init()
    {
        //Screen bounds
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenBounds.x, defaultBounds.y / screenBounds.y);

        //Camera adjustment
        CamAdjustment camAdjustment = cam.GetComponent<CamAdjustment>();
        if (camAdjustment != null)
            camAdjustment.Adjust(screenBounds);

        //Game ready
        onGameReady.Invoke();
    }

    public void SpawnUnit(GameObject reference, Vector2 position)
    {

    }

    public Vector3 ConvertToRealPos(Vector3 position)
    {
        return new Vector3(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y, 0);
    }
    public Vector2 ConvertToRealPos(Vector2 position)
    {
        return new Vector2(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y);
    }
}
