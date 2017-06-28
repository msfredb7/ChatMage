using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using FullInspector;

public class ITM_Wormholev2 : Item
{
    [InspectorHeader("Trail")]
    public float minSegmentLength = 0.3f;
    public float maxTrailLength;

    [InspectorHeader("Connect")]
    public float minConnectDistance;

    [InspectorHeader("Attack Launch")]
    public float minCircleResemblance = 0.93f;

    [NonSerialized, fsIgnore]
    private LoopTrail trail;

    public override void OnGameReady()
    {
    }

    public override void OnGameStarted()
    {
        trail = new LoopTrail(player.transform, maxTrailLength, minSegmentLength, minConnectDistance, OnTrailLooped);
        trail.debugDraw = true;
    }

    public override void OnUpdate()
    {
        if (!Game.instance.gameStarted)
            return;

        trail.Update();
    }

    void OnTrailLooped(Vector2[] points, float sectionLength)
    {
        float circleResemblance = CCC.Math.AreaWithin.ResemblanceToCircle(sectionLength / (2 * Mathf.PI), points);

        bool success = circleResemblance >= minCircleResemblance;

        if (success)
        {
            //Ceci dessine la loop en rouge. 
            // Pour la voire, il faut que tu regarde la 'scene view' et non la 'game view' pendant que tu joue
            //                                                              tip: fait 2 fen�tre

            DrawTrail(
                points,
                new Color(
                    1,
                    0,
                    0,
                    1),
                1.5f);

            //Lancer l'attaque ici !
            //1. trouver le centre de la loop (simplement faire la moyenne x,y des points)
            //2. Spawner le prefab d'explosion/trou-noir/tornade/etc.
            //3. c tou
        }
    }

    void DrawTrail(Vector2[] points, Color color, float duration)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Debug.DrawLine(points[i], points[(i + 1) % points.Length], color, duration);
        }
    }

    protected override void ClearReferences()
    {
        base.ClearReferences();
        trail = null;
    }
}
