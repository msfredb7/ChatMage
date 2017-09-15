using CCC.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteOffset))]
public class MeteoPlayer : MonoBehaviour
{
    [Header("Garder des chiffre entier")]
    public Vector2 offsetRatio = Vector2.one;
    public float offsetLoopDuration = 6;
    public bool showOnStart = false;

    private SpriteOffset sprOffset;
    private Color stdColor;
    private Tween colorTween;
    private float wasCameraHeight;
    private GameCamera gc;

    private Vector2 offset;
    private StatFloat worldTimescale;
    private float textureVertSize;

    private void Awake()
    {
        sprOffset = GetComponent<SpriteOffset>();
        stdColor = sprOffset.Color;
        textureVertSize = transform.lossyScale.y;
    }

    private void Start()
    {
        if (showOnStart)
            Show(0);
        else
            Hide(0);
    }

    private void Update()
    {
        if (GameCam != null)
        {
            //Update offset
            offset += offsetRatio * worldTimescale * Time.deltaTime / offsetLoopDuration;
            if (offset.x > offsetRatio.x)
                offset -= offsetRatio;


            float cameraHeight = GameCam.Height;
            float delta = cameraHeight - wasCameraHeight;
            if (delta != 0)
                offset += Vector2.up * ((cameraHeight - wasCameraHeight) / textureVertSize);
            wasCameraHeight = cameraHeight;


            sprOffset.Offset = offset;
        }
    }

    public void Show(float duration = 1.5f)
    {
        ColorGoTo(duration, stdColor);
    }

    public void Hide(float duration = 1.5f)
    {
        ColorGoTo(duration, stdColor.ChangedAlpha(0));
    }

    private void ColorGoTo(float duration, Color destination)
    {
        KillColor();
        if (duration <= 0)
        {
            sprOffset.Color = destination;
        }
        else
        {
            colorTween = sprOffset.DOColor(destination, duration);
        }
    }

    private void KillColor()
    {
        if (colorTween != null && colorTween.IsPlaying())
            colorTween.Kill();
    }

    GameCamera GameCam
    {
        get
        {
            if (gc == null && Game.instance != null)
            {
                gc = Game.instance.gameCamera;
                worldTimescale = Game.instance.worldTimeScale;
            }
            return gc;
        }
    }
}
