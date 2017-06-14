using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public Mapping mapping; // limite de la map, waypoints, etc.
    public CameraSpawn cameraSpawn;

    [SerializeField]
    private List<GameObject> mapObjectsToAjust;

    [Header("Optional")]
    public RoadPlayer roadPlayer;

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
	public void Init(PlayerController player)
    {
        if (roadPlayer != null)
            roadPlayer.Init(player.transform);

        for (int i = 0; i < mapObjectsToAjust.Count; i++)
        {
            Adjust(mapObjectsToAjust[i]);
        }
    }

    public void Adjust(GameObject obj)
    {
        if (obj != null)
            obj.transform.position = Game.instance.gameCamera.AdjustVector(obj.transform.position);
    }
}
