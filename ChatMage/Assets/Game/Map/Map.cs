using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [SerializeField]
    private MapInfo mapInfo; // information a propos de la map
    [SerializeField]
    private Mapping map; // limite de la map, waypoints, etc.
    [SerializeField]
    private Playground playground; // Zone jouable

    [SerializeField]
    private List<GameObject> mapObjects;

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
	void Init (float height, float width) {
        foreach(GameObject obj in mapObjects)
            Adjust(obj);

        map.Init(height, width);

        // On va chercher le playground et on set ca dans mapping
        map.SetOffsets(playground.GetTopLimit(),
                       playground.GetBottomLimit(),
                       playground.GetRightLimit(),
                       playground.GetLeftLimit());

        // Ajustement de la map a faire en fonction du height et width
	}

    private void Adjust(GameObject obj)
    {

    }
}
