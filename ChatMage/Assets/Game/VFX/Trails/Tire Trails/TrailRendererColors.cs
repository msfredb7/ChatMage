using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererColors
{
    //public float transitionLength = 0.01f;
    //private TrailRenderer renderer;
    //private Transform tr;

    //private GradientAlphaKey[] aKeys;
    //private GradientColorKey[] cKeys;
    //private Gradient gradient;

    //private Color startColor;
    //private Color endColor;

    //private LinkedList<GradientColorKey> colors;


    //public TrailRendererColors(TrailRenderer trailRenderer, Transform followTarget)
    //{
    //    renderer = trailRenderer;
    //    tr = followTarget;

    //    gradient = renderer.colorGradient;

    //    cKeys = gradient.colorKeys;
    //    aKeys = gradient.alphaKeys;
    //}

    //public void SetColor(Color color)
    //{
    //    if (colors.First != null && colors.First.Value.time < 0)
    //        colors.First.Value = new GradientColorKey(GetStartColor(), 0);
    //    else
    //        colors.AddFirst(new GradientColorKey(GetStartColor(), 0));

    //    colors.AddFirst(new GradientColorKey(color, transitionLength))
    //}

    //private float RealToNorm(float value)
    //{
    //    return value * renderer.Getpo
    //}

    //float GetTrailLength()
    //{
    //    float total = 0;
    //    Vector3[] positions = null;
    //    renderer.GetPositions(positions);

    //    for (int i = 0; i < positions.Length; i++)
    //    {
    //        total += positions
    //    }
    //}

    //public void UpdateColors()
    //{


    //    gradient.SetKeys(cKeys, aKeys);
    //    renderer.colorGradient = gradient;
    //}

    //private void BuildKeys(out GradientColorKey[] colorKeys)
    //{
    //    LinkedListNode<GradientColorKey> node = colors.First;

    //    int count = colors.Count;
    //    if (colors.First != null && IsOut(colors.First.Value.time))
    //        count--;
    //    if (colors.Last != null && IsOut(colors.Last.Value.time))
    //        count--;
    //    if (count < 0)
    //        count = 0;

    //    //Pour mettre les 'start color' et 'end color'
    //    count += 2;

    //    colorKeys = new GradientColorKey[count];
    //    int i = 1;

    //    //Start color
    //    colorKeys[0] = new GradientColorKey(GetStartColor(), 0);

    //    //Tous les couleurs entre
    //    while (node != null)
    //    {
    //        if (!IsOut(node.Value.time))
    //        {
    //            colorKeys[i] = node.Value;
    //            i++;
    //        }

    //        node = node.Next;
    //    }

    //    //Couleur finale
    //    colorKeys[i] = new GradientColorKey(GetEndColor(), 1);
    //}

    //private bool IsOut(float time)
    //{
    //    return time < 0 || time > 1;
    //}

    //private Color GetStartColor()
    //{
    //    if (colors.Count == 0)
    //        return Color.white;
    //    if (colors.Count == 1)
    //        return colors.First.Value.color;

    //    return LerpBetweenDuo(colors.First, colors.First.Next, 0);
    //}

    //private Color GetEndColor()
    //{
    //    if (colors.Count == 0)
    //        return Color.white;
    //    if (colors.Count == 1)
    //        return colors.First.Value.color;

    //    return LerpBetweenDuo(colors.Last.Previous, colors.Last, 1);
    //}

    //private Color LerpBetweenDuo(LinkedListNode<GradientColorKey> first, LinkedListNode<GradientColorKey> last, float time)
    //{
    //    float length = last.Value.time - first.Value.time;
    //    return Color.Lerp(first.Value.color, last.Value.color, Mathf.Clamp01(time - colors.First.Value.time / length));
    //}
}
