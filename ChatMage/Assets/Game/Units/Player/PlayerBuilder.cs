using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour {

    [Header("Temporaire")]
    public Vehicle playerPrefab;
    
    /// <summary>
    /// TODO: Prendre le loadout en parametre
    /// </summary>
    public Vehicle BuildPlayer()
    {
        return Instantiate(playerPrefab.gameObject).GetComponent<Vehicle>();
    }
}
