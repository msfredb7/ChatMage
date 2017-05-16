using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : PublicSingleton<Game>
{
    public Camera cam;

    public float Aspect { get { return cam.aspect; } }
    public Vector2 ScreenBounds { get { return screenBounds; } }
    private Vector2 screenBounds;

    public UnityEvent onGameReady = new UnityEvent();
    public UnityEvent onGameStarted = new UnityEvent();

    public List<GameObject> units = new List<GameObject>();

    public void Init()
    {
        //Screen bounds
        screenBounds = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);

        //Camera adjustment
        CamAdjustment camAdjustment = cam.GetComponent<CamAdjustment>();
        if (camAdjustment != null)
            camAdjustment.Adjust(screenBounds);

        //Game ready
        onGameReady.Invoke();
        print("hello");
    }

    public void SpawnUnit(GameObject reference, Vector2 position)
    {

    }
}
