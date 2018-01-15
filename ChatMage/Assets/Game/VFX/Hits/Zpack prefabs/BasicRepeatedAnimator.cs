using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRepeatedAnimator : MonoBehaviour
{
    public Animator animator;
    int speedHash = Animator.StringToHash("speed");
    int endHash = Animator.StringToHash("end");
    float timescale = 1;

    void Start()
    {
        if (Game.Instance.worldTimeScale != 1)
            UpdateTimescale(Game.Instance.worldTimeScale);

        Game.Instance.worldTimeScale.onSet.AddListener(UpdateTimescale);
    }

    void OnDestroy()
    {
        if (Game.Instance != null)
            Game.Instance.worldTimeScale.onSet.RemoveListener(UpdateTimescale);
    }

    void UpdateTimescale(float value)
    {
        timescale = value;
        ApplyTimescale();
    }

    void ApplyTimescale()
    {
        if (animator.isActiveAndEnabled)
            animator.SetFloat(speedHash, timescale);
    }

    public void Animate(Vector2 position)
    {
        gameObject.SetActive(true);
        ApplyTimescale();
        transform.position = position;
    }

    void OnAnimationEnd()
    {
        animator.SetTrigger(endHash);
        gameObject.SetActive(false);
    }

    public bool IsPlaying { get { return gameObject.activeSelf; } }
}
