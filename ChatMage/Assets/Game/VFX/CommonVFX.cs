using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonVFX : MonoBehaviour
{
    [SerializeField]
    private BasicRepeatedAnimator mediumHit;
    [SerializeField]
    private BasicRepeatedAnimator smallHit;

    private List<BasicRepeatedAnimator> mediumHits = new List<BasicRepeatedAnimator>();
    private List<BasicRepeatedAnimator> smallHits = new List<BasicRepeatedAnimator>();

    public void MediumHit(Vector2 position, Color color, int sortingLayer = SortingLayers.DEFAULT)
    {
        BasicRepeatedAnimator vfx = GetUnactiveFrom(mediumHits, mediumHit);

        SpriteRenderer renderer = vfx.GetComponent<SpriteRenderer>();
        renderer.color = color;
        renderer.sortingLayerID = SortingLayer.layers[sortingLayer].id;

        vfx.Animate(position);
    }

    public void SmallHit(Vector2 position, Color color, int sortingLayer = SortingLayers.DEFAULT)
    {
        BasicRepeatedAnimator vfx = GetUnactiveFrom(smallHits, smallHit);

        SpriteRenderer renderer = vfx.GetComponent<SpriteRenderer>();
        renderer.color = color;
        renderer.sortingLayerID = SortingLayer.layers[sortingLayer].id;

        vfx.Animate(position);
    }

    private BasicRepeatedAnimator GetUnactiveFrom(List<BasicRepeatedAnimator> list, BasicRepeatedAnimator prefabReference)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].IsPlaying)
                return list[i];
        }

        //Spawn new animator
        BasicRepeatedAnimator newAnimator = Instantiate(prefabReference.gameObject,
            Game.Instance.unitsContainer)
            .GetComponent<BasicRepeatedAnimator>();

        list.Add(newAnimator);

        return newAnimator;
    }
}
