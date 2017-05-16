using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private MapInfo mapInfo;
    private Mapping map;
    private Playground playground;

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
	void Init (float height, float width) {
        map.Init(height, width);

        // On va chercher le playground et on set ca dans mapping
        map.SetOffsets(playground.GetTopLimit(),
                       playground.GetBottomLimit(),
                       playground.GetRightLimit(),
                       playground.GetLeftLimit());

        // Ajustement de la map a faire en fonction du height et width
	}
}
