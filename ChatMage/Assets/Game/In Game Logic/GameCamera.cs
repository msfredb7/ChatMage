using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Camera cam;
    public VectorShaker vectorShaker;

    public float Aspect { get { return cam.aspect; } }

    [Header("Settings")]
    public Vector2 defaultBounds;
    public float distance = -10;
    public float minHeight = float.NegativeInfinity;
    public float maxHeight = float.PositiveInfinity;
    public bool canScrollUp = true;
    public bool canScrollDown = true;

    [Header("Follow")]
    public bool followPlayer = false;
    public float maxTurnSpeed = 2;
    public float lerpSpeed = 1;
    public float followForwardDistance = 2;

    public bool MovedSinceLastFrame { get { return movedSinceLastFrame; } }

    private bool movedSinceLastFrame = false;
    private Vector2 screenSize;
    private Vector2 defaultToRealRatio;
    private Transform tr;
    private PlayerVehicle player;

    //Follow
    private Vector2 forwardVector;
    private float followTargetDeltaHeight = 0;
    private float playerSmoothPosition;

    //Teleport follow
    private float lastKnownPlayerHeight;
    private bool isTeleporting = false;

    void Awake()
    {
        tr = transform;
    }

    public void Init(PlayerVehicle player)
    {
        this.player = player;

        //Screen bounds
        screenSize = new Vector2(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2);
        defaultToRealRatio = new Vector2(defaultBounds.x / screenSize.x, defaultBounds.y / screenSize.y);

        //Spawn a la bonne hauteur
        SetToHeight(Game.instance.map.cameraSpawn.Height);
    }

    public void OnCompleteTeleport()
    {
        isTeleporting = false;

        //Il faut faire ca, sinon, on pert une frame sur le joueur
        FixedUpdate();
    }

    public void OnTeleport(float deltaY)
    {
        playerSmoothPosition += deltaY;
        SetToHeight(TargetHeight);
        isTeleporting = true;
    }

    public void CenterOnPlayer()
    {
        SetToHeight(player.Position.y);
    }

    public void SetToHeight(float height)
    {
        height = Mathf.Clamp(height, minHeight, maxHeight);

        if (!canScrollUp)
            height = Mathf.Min(tr.position.y, height);
        if (!canScrollDown)
            height = Mathf.Max(tr.position.y, height);

        tr.position = new Vector3(0, height, distance);
        movedSinceLastFrame = true;
    }

    void Update()
    {
        //Camera Shake
        cam.transform.localPosition = vectorShaker.CurrentVector;

        //if (Input.GetKeyDown(KeyCode.T))
        //    vectorShaker.Hit(Vector2.up * 0.2f);
    }

    void FixedUpdate()
    {
        if (isTeleporting)
            return;

        if (player != null && player.gameObject.activeSelf)
        {
            if (followPlayer)
            {
                playerSmoothPosition = Mathf.MoveTowards(playerSmoothPosition, player.Position.y, Time.deltaTime * 10);

                forwardVector = Vector2.MoveTowards(forwardVector, player.WorldDirection2D() * followForwardDistance, Time.fixedDeltaTime * maxTurnSpeed);
                followTargetDeltaHeight = Mathf.Lerp(followTargetDeltaHeight, forwardVector.y, FixedLerp.FixedFix(0.01f * lerpSpeed));

                SetToHeight(TargetHeight);
            }
            else
            {
                forwardVector = Center - player.Position;
                followTargetDeltaHeight = Height - player.Position.y;
            }
        }
        //else
        //{
        //    followTargetDeltaHeight = 0;
        //}

        playerSmoothPosition = Height - followTargetDeltaHeight;
    }

    private float TargetHeight { get { return playerSmoothPosition + followTargetDeltaHeight; } }

    #region Bounds

    public float Top { get { return Height + screenSize.y / 2; } }
    public float Bottom { get { return Height - screenSize.y / 2; } }
    public float Left { get { return -screenSize.x / 2; } }
    public float Right { get { return screenSize.x / 2; } }
    public Vector2 Center { get { return new Vector2(0, Height); } }
    public float Height { get { return tr.position.y; } }
    public Vector2 ScreenSize { get { return screenSize; } }

    public Vector3 AdjustVector(Vector3 position)
    {
        return new Vector3(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y, position.z);
    }
    public Vector2 AdjustVector(Vector2 position)
    {
        return new Vector2(position.x / defaultToRealRatio.x, position.y / defaultToRealRatio.y);
    }

    #endregion
}
