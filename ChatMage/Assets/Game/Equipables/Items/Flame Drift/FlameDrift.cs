using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDrift : MonoBehaviour {

    public TrailRenderer trailPrefab;
    public string sortingLayer;

    private Transform currentTrail;

    public void StartTrail()
    {
        if (currentTrail != null)
            return;

        currentTrail = Instantiate(trailPrefab.gameObject, transform).transform;
        currentTrail.GetComponent<TrailRenderer>().sortingLayerName = sortingLayer;
    }

    public void EndTrail()
    {
        currentTrail.SetParent(Game.instance.unitsContainer);
        currentTrail = null;
    }
}
