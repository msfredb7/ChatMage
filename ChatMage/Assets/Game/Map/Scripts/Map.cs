using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;
using FullInspector;
using FullSerializer;

public class Map : BaseBehavior
{
    [InspectorMargin(10), InspectorHeader("Waypoints Manager")]
    public Mapping mapping; // limite de la map, waypoints, etc.

    [InspectorMargin(10), InspectorHeader("Camera")]
    public CameraSpawn cameraSpawn;
    public bool showCameraTrails = true;

    [InspectorMargin(10), InspectorHeader("Units Already Spawned")]
    public List<GameObject> listUnits = new List<GameObject>();

    [InspectorMargin(10), SerializeField, fsProperty, InspectorHeader("Object Needing Camera Adjustement")]
    private List<GameObject> objectsToAjust = new List<GameObject>();

    [InspectorMargin(10), InspectorHeader("AI Area")]
    public bool setAIArea = false;
    [InspectorShowIf("setAIArea")]
    public Box2D startAIArea;
    [InspectorShowIf("setAIArea")]
    public bool ajustAreaToCamera = true;

    [InspectorMargin(10), InspectorHeader("Optional")]
    public RoadPlayer roadPlayer;
    public PositionDisplacer[] positionDisplacers;

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
	public void Init(PlayerController player)
    {
        if (roadPlayer != null)
            roadPlayer.Init(player.transform);

        for (int i = 0; i < objectsToAjust.Count; i++)
        {
            Adjust(objectsToAjust[i]);
        }

        for (int i = 0; i < listUnits.Count; i++)
        {
            Unit unit = listUnits[i].GetComponent<Unit>();
            if (unit != null)
                Game.instance.AddExistingUnit(unit);
        }

        if (setAIArea)
        {
            ResetAIArea();
        }

        mapping.Init(Game.instance);
    }

    public void Adjust(GameObject obj)
    {
        if (obj != null)
            obj.transform.position = Game.instance.gameCamera.AdjustVector(obj.transform.position);
    }

    public void ResetAIArea()
    {
        GameCamera gameCamera = Game.instance.gameCamera;
        startAIArea.min = gameCamera.AdjustVector(startAIArea.min);
        startAIArea.max = gameCamera.AdjustVector(startAIArea.max);
        Game.instance.aiArea.SetArea(startAIArea);
    }

    public void OnDrawGizmosSelected()
    {
        if (setAIArea)
        {
            Gizmos.color = new Color(0.2f, 1, 0, 0.4f);
            Gizmos.DrawCube(startAIArea.Center, startAIArea.Size);
        }
    }
    public void OnDrawGizmos()
    {
        if (showCameraTrails)
        {
            float halfSC = GameCamera.DEFAULT_SCREEN_WIDTH / 2;

            //Camera size display
            Gizmos.color = new Color(1, 0, 1, 1);

            //Left side
            Vector2 start = new Vector2(-halfSC, 900);
            Vector2 end = new Vector2(-halfSC, -900);
            Gizmos.DrawLine(start, end);

            //Right side
            start = new Vector2(halfSC, 900);
            end = new Vector2(halfSC, -900);
            Gizmos.DrawLine(start, end);


            Gizmos.color = new Color(1, 0, 1, 0.3f);

            //Left side (including shake)
            start = new Vector2(-halfSC - 1.5f, 900);
            end = new Vector2(-halfSC - 1.5f, -900);
            Gizmos.DrawLine(start, end);

            //Right side (including shake)
            start = new Vector2(halfSC + 1.5f, 900);
            end = new Vector2(halfSC + 1.5f, -900);
            Gizmos.DrawLine(start, end);
        }
    }

    public Vector2 VerifyPosition(Vector2 pos, float unitWidth)
    {
        Vector2 newPos = pos;
        if (positionDisplacers != null)
            for (int i = 0; i < positionDisplacers.Length; i++)
            {
                if (positionDisplacers[i].Displace(pos, unitWidth, out newPos))
                    break;
            }

        return newPos;
    }
}
