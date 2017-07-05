using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    [Header("Waypoints Manager")]
    public Mapping mapping; // limite de la map, waypoints, etc.

    [Header("Camera")]
    public CameraSpawn cameraSpawn;

    [Header("Units Already Spawned")]
    public List<GameObject> listUnits = new List<GameObject>();

    [SerializeField, Header("Object Needing Camera Adjustement")]
    private List<GameObject> objectsToAjust = new List<GameObject>();

    [Header("Optional")]
    public RoadPlayer roadPlayer;

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

        mapping.Init(Game.instance);
    }

    public void Adjust(GameObject obj)
    {
        if (obj != null)
            obj.transform.position = Game.instance.gameCamera.AdjustVector(obj.transform.position);
    }
}
