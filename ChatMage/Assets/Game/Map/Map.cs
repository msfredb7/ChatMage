using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private MapInfo mapInfo;
    private Mapping map;

    /// <summary>
    /// Initialise les settings de la map
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
	void Init (float height, float width) {
        map.Init(height, width);
	}


}
