using UnityEngine;
using System.Collections;
using DG.Tweening;
using CCC.Utility;

public class SmashBallAnimator : MonoBehaviour
{
    public SpriteRenderer[] ballRenderers;
    [Header("Unhittable"), Range(1, 20)]
    public float flashSpeed = 5;
    public float unhitableDuration = 1;

    [Header("Ray Color")]
    public SpriteRenderer coloredRay;
    public SpriteRenderer ambiantLight;

    [Header("Death anim")]
    public SpriteRenderer coreSprite;
    public GameObject deathAnimPrefab;
    public float destroyDelay = 1;
    public Vector3 attachedToPlayerSize = new Vector3(1.3f, 0.6f, 1);
    public Collider2D col;

    SmashBall ball;
    Sequence unhitableSequence;

    void Start()
    {
        ball = GetComponent<SmashBall>();
        ball.onHitPlayer += Ball_onHitPlayer;
        ball.onTimeScaleChange += Ball_onTimeScaleChange;
        ball.OnDeath += Ball_onDeath;
        AnimateRayColor();
    }

    private void Ball_onDeath(Unit unit)
    {
        //On arrete de bouger
        ball.Speed = Vector2.zero;
        ball.enabled = false;
        ball.rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;

        //Desactive les renderers
        coreSprite.enabled = false;

        //On active la death anim
        Instantiate(deathAnimPrefab.gameObject, ball.Position, Quaternion.identity, Game.instance.unitsContainer);

        //On s'attache au joueur
        if (Game.instance.Player != null)
        {
            transform.localScale = Vector3.zero;
            transform.SetParent(Game.instance.Player.body);
            transform.DOScale(attachedToPlayerSize, 0.5f);
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            Game.instance.Player.playerSmash.onSmashStarted += Disappear;
        }
        else
            Disappear();
    }

    void Disappear()
    {
        transform.DOScale(0, 0.5f).OnComplete(delegate ()
        {
            //On se detruit
            Destroy(gameObject);
        });
    }

    void OnDestroy()
    {
        //Remove listeners
        if(Game.instance != null && Game.instance.Player != null)
        {
            Game.instance.Player.playerSmash.onSmashStarted -= Disappear;
        }

        if (unhitableSequence != null)
            unhitableSequence.Kill();
    }

    private void Ball_onTimeScaleChange(Unit unit)
    {
        if (unhitableSequence != null)
            unhitableSequence.timeScale = unit.TimeScale;
    }

    private void Ball_onHitPlayer()
    {
        if (ball.hp <= 0)
            return;

        ball.CanHit = false;

        unhitableSequence = DOTween.Sequence();
        unhitableSequence.InsertCallback(
            (1 / flashSpeed) / 2,
            delegate
            {
                SetVisible(false);
            });
        unhitableSequence.InsertCallback(
            1 / flashSpeed,
            delegate
            {
                SetVisible(true);
            });
        unhitableSequence.SetUpdate(false);
        unhitableSequence.SetLoops(Mathf.RoundToInt(unhitableDuration * flashSpeed), LoopType.Restart);
        unhitableSequence.OnComplete(delegate ()
        {
            ball.CanHit = true;
        });
    }

    public void SetVisible(bool state)
    {
        for (int i = 0; i < ballRenderers.Length; i++)
        {
            ballRenderers[i].enabled = state;
        }
    }

    public bool IsVisible()
    {
        for (int i = 0; i < ballRenderers.Length; i++)
        {
            // Si un seul des renderers n'est pas enable, la smashball n'est pas visible
            if (!ballRenderers[i].enabled)
                return false;
        }
        // Sinon elle est visible
        return true;
    }

    private ColorHSV rayColor;

    public void AnimateRayColor()
    {
        rayColor = new ColorHSV(0, 1, 1);
        DOTween.To(
            () => rayColor.h,
            (x) =>
            {
                rayColor.h = x;
                Color c = (Color)rayColor;
                coloredRay.color = c;
                ambiantLight.color = Color.Lerp(Color.white, c, 0.5f);
            },
            1,
            2)
            .SetLoops(-1, LoopType.Restart);
    }
}
