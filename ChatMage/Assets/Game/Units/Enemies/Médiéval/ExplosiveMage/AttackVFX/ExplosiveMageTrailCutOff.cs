using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMageTrailCutOff : MonoBehaviour
{
    public Traveler traveler;
    public TrailRenderer trail;

    [SerializeField] bool _cutOff = false;
    private Vector3 cutOffPosition;
    private Transform tr;

    GradientAlphaKey[] alphaKeys;
    GradientColorKey[] colorKeys;

    Gradient gradient = new Gradient();

    void Awake()
    {
        tr = transform;
        alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1,0),
            new GradientAlphaKey(1,0),
            new GradientAlphaKey(0,1)
        };
        colorKeys = trail.colorGradient.colorKeys;
        gradient.SetKeys(colorKeys, alphaKeys);
        trail.colorGradient = gradient;
    }

    void Update()
    {
        UpdateGradient();
    }

    void UpdateGradient()
    {
        if (_cutOff)
        {
            var delta = (tr.position - cutOffPosition).magnitude;
            var maxDistance = trail.time * traveler.TravelSpeed;
            SetGradientCutOff01(Mathf.Clamp01(delta / maxDistance));
        }
        else
        {
            SetGradientCutOff01(0);
        }
    }

    void SetGradientCutOff01(float time)
    {
        GradientAlphaKey key = new GradientAlphaKey(1 - time, time);
        GradientAlphaKey zeroKey = new GradientAlphaKey(time > 0 ? 0 : 1, Mathf.Clamp01(key.time - 0.01f));
        alphaKeys[0] = zeroKey;
        alphaKeys[1] = key;

        gradient.SetKeys(colorKeys, alphaKeys);
        trail.colorGradient = gradient;
    }
    public void CutOff()
    {
        _cutOff = true;
        cutOffPosition = tr.position;
        UpdateGradient();
    }
    public void CutOff(Vector3 atPosition)
    {
        _cutOff = true;
        cutOffPosition = atPosition;
        UpdateGradient();
    }
}
