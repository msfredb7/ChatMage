using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlicker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Vector2 range;
    public float speed;

    Color originalColor;
    private float timer;

    void OnEnable()
    {
        originalColor = spriteRenderer.color;
    }

    void OnDisable()
    {
        spriteRenderer.color = originalColor;
    }

    void Update()
    {
        timer += speed * Time.deltaTime;
        spriteRenderer.color = originalColor.ChangedAlpha(Mathf.Clamp01(Mathf.PerlinNoise(timer, 666) * RangeSize + MinAlpha));
    }

    float MinAlpha { get { return range.x; } }
    float RangeSize { get { return range.y - range.x; } }
}
