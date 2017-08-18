using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRepeatedAnimator : MonoBehaviour
{
    public Animator animator;
    int speedHash = Animator.StringToHash("speed");
    int endHash = Animator.StringToHash("end");

    void Start()
    {
        if (Game.instance.worldTimeScale != 1)
            UpdateTimescale(Game.instance.worldTimeScale);

        Game.instance.worldTimeScale.onSet.AddListener(UpdateTimescale);
    }

    void UpdateTimescale(float value)
    {
        animator.SetFloat(speedHash, value);
    }

    public void Animate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    void OnAnimationEnd()
    {
        animator.SetTrigger(endHash);
        gameObject.SetActive(false);
    }

    public bool IsPlaying { get { return gameObject.activeSelf; } }
}
