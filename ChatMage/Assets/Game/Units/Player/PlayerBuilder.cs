using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour {

    [Header("Temporaire")]
    public PlayerController playerPrefab;
    
    /// <summary>
    /// TODO: Prendre le loadout en parametre
    /// </summary>
    public Vehicle BuildPlayer()
    {
        PlayerController playerController = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();
        playerController.Init();

        return playerController.GetComponent<Vehicle>();
    }
}
