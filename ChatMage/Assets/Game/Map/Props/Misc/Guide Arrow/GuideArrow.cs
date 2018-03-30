using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuideArrow : MonoBehaviour
{
    [Header("Linking")]
    public SpriteRenderer spriteRenderer;

    [Header("Animation Settings")]
    public float bounceDistance;
    public Vector2 direction = Vector2.down;
    public Ease bounceEase = Ease.OutSine;
    public float bounceDuration = 0.35f;

    protected Tween tween;
    private float timer = -1;

    Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        timer = -1;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        transform.position = initialPos;
        timer = -1;
    }

    public void ShowFor(float time)
    {
        gameObject.SetActive(true);
        timer = time;
    }

    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                Hide();
        }
    }

    void OnEnable()
    {
        Animate();
    }

    void OnDisable()
    {
        KillTween();
    }

    void Animate()
    {
        KillTween();

        Transform tr = transform;
        Vector2 currentPos = tr.position;
        Vector2 destination = currentPos + (direction.normalized * -bounceDistance);
        tween = tr.DOMove(destination, bounceDuration).SetEase(bounceEase).SetLoops(-1, LoopType.Yoyo);
    }

    protected void KillTween()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }
}
