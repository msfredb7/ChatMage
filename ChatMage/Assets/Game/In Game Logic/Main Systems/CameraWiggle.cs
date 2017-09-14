using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWiggle : MonoBehaviour
{
    public float animationSize = 0.2f;
    public float playerSpaceToScreenRatio = .7f;

    private Transform pt;
    private Transform tr;
    private Vector2 offset;
    private Vector2 playerSpace;

    private void Awake()
    {
        tr = transform;
        playerSpace = new Vector2(GameCamera.DEFAULT_SCREEN_WIDTH / 2 * playerSpaceToScreenRatio,
            GameCamera.DEFAULT_SCREEN_HEIGHT / 2 * playerSpaceToScreenRatio);
    }

    private void Update()
    {
        if (Player != null)
        {
            Vector2 playerToScreen = Player.position - tr.position;
            Vector2 displacement = new Vector2((playerToScreen.x / playerSpace.x).Clamped(-1, 1),
                (playerToScreen.y / playerSpace.y).Clamped(-1, 1));

            offset = displacement * animationSize;
        }
    }

    public Vector2 CurrentOffset { get { return offset; } }

    private Transform Player
    {
        get
        {
            if (pt == null && Game.instance != null && Game.instance.Player != null)
                pt = Game.instance.Player.transform;
            return pt;
        }
    }
}
