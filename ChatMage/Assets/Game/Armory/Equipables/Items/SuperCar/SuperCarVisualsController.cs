using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCarVisualsController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform[] nitroTransforms;
    [SerializeField] SpriteRenderer nitroLight;

    [Header("Settings")]
    [SerializeField] AnimationCurve nitroPowerCurve;
    Vector3 nitroMaxSize;
    float nitroLightMaxAlpha;

    void Awake()
    {
        nitroMaxSize = nitroTransforms[0].localScale;
        nitroLightMaxAlpha = nitroLight.color.a;
    }

    public void SetSpriteActive(bool state)
    {
        sprite.enabled = state;
    }

    public void SetNitroPower01(float value)
    {
        value = nitroPowerCurve.Evaluate(1 - value);

        for (int i = 0; i < nitroTransforms.Length; i++)
        {
            nitroTransforms[i].localScale = nitroMaxSize * value;
        }

        nitroLight.SetAlpha(nitroLightMaxAlpha * value);
    }
}
