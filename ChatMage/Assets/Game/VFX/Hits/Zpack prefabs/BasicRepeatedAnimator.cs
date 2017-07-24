using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRepeatedAnimator : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        if (Game.instance.worldTimeScale != 1)
            UpdateTimescale(Game.instance.worldTimeScale);

        Game.instance.worldTimeScale.onSet.AddListener(UpdateTimescale);
    }

    void UpdateTimescale(float value)
    {
        animator.SetFloat("speed", value);
    }

    public void Animate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    void OnAnimationEnd()
    {
        gameObject.SetActive(false);
        animator.SetTrigger("end");
    }

    public bool IsPlaying { get { return gameObject.activeSelf; } }
}
