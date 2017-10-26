using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDrift))]
public class PlayerDriftTrails : MonoBehaviour
{
    [Header("Trail Renderers")]
    public string sortingLayer;
    public bool spawnStdTrails = true;
    public TrailRenderer stdTrailPrefab;
    public bool spawnDriftTrails = true;
    public TrailRenderer driftTrailPrefab;

    private Transform[] trails;

    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        PlayerDrift pd = GetComponent<PlayerDrift>();
        pd.onStartDrift += BeginTrails;
        pd.onEndDrift += DetachTrails;

        trails = new Transform[controller.playerLocations.wheels.Length];

        if (spawnStdTrails)
        {
            NewTrail(stdTrailPrefab, controller.playerLocations.BackLeftWheel.position);
            NewTrail(stdTrailPrefab, controller.playerLocations.BackRightWheel.position);
        }
    }

    private void BeginTrails()
    {
        for (int i = 0; i < controller.playerLocations.wheels.Length; i++)
        {
            //Detache l'ancienne trail
            if (trails[i] != null)
                continue;

            //Nouvelle trail
            trails[i] = NewTrail(driftTrailPrefab, controller.playerLocations.wheels[i].position);
        }
    }

    private void DetachTrails()
    {
        for (int i = 0; i < controller.playerLocations.wheels.Length; i++)
        {
            //Detache l'ancienne trail
            if (trails[i] != null)
            {
                trails[i].SetParent(Game.instance.unitsContainer);
                trails[i] = null;
            }
        }
    }

    private Transform NewTrail(TrailRenderer prefab, Vector3 worldPosition)
    {
        TrailRenderer newTrail = Instantiate(prefab.gameObject, worldPosition, Quaternion.identity, controller.body)
            .GetComponent<TrailRenderer>();
        newTrail.sortingLayerName = sortingLayer;
        return newTrail.transform;
    }
}
