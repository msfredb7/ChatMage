using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public const float DEFAULT_SCREEN_WIDTH = 19.2f;
    public const float DEFAULT_SCREEN_HEIGHT = 10.8f;
    public const float MAX_CAMERA_SHAKE = 1.2f;

    public Camera cam;
    public VectorShaker vectorShaker;

    [Header("Settings")]
    public float distance = -10;
    public float minHeight = float.NegativeInfinity;
    public float maxHeight = float.PositiveInfinity;
    public bool canScrollUp = true;
    public bool canScrollDown = true;

    [Header("Follow")]
    public bool followPlayer = false;
    public float followForwardDistance = 2;
    public float acceleration = 1;
    public float decelerationSpeedFactor = 1;

    public bool MovedSinceLastFrame { get { return movedSinceLastFrame; } }

    private bool movedSinceLastFrame = false;
    private Vector2 screenSize = new Vector2(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);
    private Transform tr;
    private PlayerVehicle player;

    //Teleport follow
    private bool isTeleporting = false;

    //Follow
    private float verticalSpeed = 0;

    void Awake()
    {
        tr = transform;

        //Screen bounds
        float aspect = cam.aspect;
        float width = aspect * cam.orthographicSize * 2;
        cam.orthographicSize *= DEFAULT_SCREEN_WIDTH / width;
        vectorShaker.max = MAX_CAMERA_SHAKE;
    }

    public void Init(PlayerVehicle player)
    {
        this.player = player;

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
        Debug.LogWarning("Teleportation non support�. La camera va probablement agir bizarrement.");
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
    }

    void FixedUpdate()
    {
        if (isTeleporting)
            return;

        if (followPlayer && player != null && player.gameObject.activeSelf)
        {
                float hg = Height;

                float playerHeight = player.Position.y;
                float dest = playerHeight + player.WorldDirection2D().y * followForwardDistance;

                float delta = dest - hg;

                float targetSpeed = delta.Sign() * float.PositiveInfinity;

                verticalSpeed = verticalSpeed.MovedTowards(targetSpeed, acceleration * Time.fixedDeltaTime);


                if (delta > 0)
                    verticalSpeed = verticalSpeed.Capped(delta * decelerationSpeedFactor);
                else
                    verticalSpeed = verticalSpeed.Floored(delta * decelerationSpeedFactor);


                SetToHeight(hg + (verticalSpeed * Time.fixedDeltaTime));

                verticalSpeed = (Height - hg) / Time.fixedDeltaTime;
        }
        else
        {
            verticalSpeed = 0;
        }
    }

    #region Bounds

    public float Aspect { get { return DEFAULT_SCREEN_WIDTH / DEFAULT_SCREEN_HEIGHT; } }
    public float Top { get { return Height + screenSize.y / 2; } }
    public float Bottom { get { return Height - screenSize.y / 2; } }
    public float Left { get { return -screenSize.x / 2; } }
    public float Right { get { return screenSize.x / 2; } }
    public Vector2 Center { get { return new Vector2(0, Height); } }
    public float Height { get { return tr.position.y; } }
    public Vector2 ScreenSize { get { return screenSize; } }
    public Vector2 BottomLeft { get { return new Vector2(Bottom, Left); } }
    public Vector2 BottomRight { get { return new Vector2(Bottom, Right); } }
    public Vector2 TopLeft { get { return new Vector2(Top, Left); } }
    public Vector2 TopRight { get { return new Vector2(Top, Right); } }

    public Vector3 AdjustVector(Vector3 vector)
    {
        return vector;
    }
    public Vector2 AdjustVector(Vector2 vector)
    {
        return vector;
    }
    public Vector2 ClampToScreen(Vector2 position, float screenSizeMultiplier = 1)
    {
        Vector2 center = Center;
        Vector2 halfSS = ScreenSize * screenSizeMultiplier / 2;

        return new Vector2(
            position.x.Clamped(center.x - halfSS.x, center.x + halfSS.x),
            position.x.Clamped(center.y - halfSS.y, center.y + halfSS.y));
    }

    #endregion
}
