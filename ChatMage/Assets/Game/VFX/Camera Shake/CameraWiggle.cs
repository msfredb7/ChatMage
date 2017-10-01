using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWiggle : MonoBehaviour
{
    public const float SIZE = 0.125f;
    public float playerSpaceToScreenRatio = .7f;

    private Transform pt;
    private Transform tr;
    private Vector2 offset;
    private Vector2 playerSpace;

    private float actualSize;
    private float targetSize;
    private float transitionDuration = 1;
    private const float xMultiplier = GameCamera.DEFAULT_SCREEN_WIDTH / GameCamera.DEFAULT_SCREEN_HEIGHT;

    private void Awake()
    {
        targetSize = SIZE;
        tr = transform;

        //Note: ceci est independant de l'aspect de la camera (4:3 / 16:9)
        playerSpace = new Vector2(GameCamera.DEFAULT_SCREEN_WIDTH / 2 * playerSpaceToScreenRatio,
            GameCamera.DEFAULT_SCREEN_HEIGHT / 2 * playerSpaceToScreenRatio);
    }

    private void Update()
    {
        //Transition to target size
        if(IsInTransition)
        {
            if (transitionDuration <= 0)
            {
                actualSize = targetSize;
            }
            else
            {
                float changeSpeed = SIZE / transitionDuration;
                actualSize = actualSize.MovedTowards(targetSize, Time.deltaTime * changeSpeed);
            }
        }

        if (Player != null)
        {
            Vector2 playerToScreen = Player.position - tr.position;
            Vector2 displacement = new Vector2((playerToScreen.x / playerSpace.x).Clamped(-1, 1) * xMultiplier,
                (playerToScreen.y / playerSpace.y).Clamped(-1, 1));

            offset = displacement * actualSize;
        }
    }

    public Vector2 CurrentOffset { get { return offset; } }
    public float ActualSize { get { return actualSize; } }
    public bool IsInTransition { get { return actualSize != targetSize; } }

    public void SetAnimationSize(float animationSize, float transitionDuration)
    {
        targetSize = animationSize;
        this.transitionDuration = transitionDuration;
    }
    public float GetAnimationSize()
    {
        return targetSize;
    }

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
