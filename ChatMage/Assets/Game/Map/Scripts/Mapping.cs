using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;
using CCC.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mapping : MonoBehaviour
{
    [SerializeField, Header("Fill")]
    private List<Waypoint> waypoints;

    [SerializeField, ReadOnly(), Header("Result")]
    private List<Waypoint> enemyWaypoints;
    [SerializeField, ReadOnly()]
    private List<Waypoint> playerWaypoints;
    [SerializeField, ReadOnly()]
    private List<Waypoint> bossWaypoints;
    [SerializeField, ReadOnly()]
    private List<Waypoint> itemWaypoints;
    [SerializeField, ReadOnly()]
    private List<Waypoint> otherWaypoints;
    [SerializeField, ReadOnly()]
    private List<Waypoint> tagsWaypoints;
    
    public void BuildWaypointLists()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            switch (waypoints[i].GetWaypointType())
            {
                case Waypoint.WaypointType.enemySpawn:
                    enemyWaypoints.Add(waypoints[i]);
                    continue;
                case Waypoint.WaypointType.PlayerSpawn:
                    playerWaypoints.Add(waypoints[i]);
                    continue;
                case Waypoint.WaypointType.BossSpawn:
                    bossWaypoints.Add(waypoints[i]);
                    continue;
                case Waypoint.WaypointType.items:
                    itemWaypoints.Add(waypoints[i]);
                    continue;
                case Waypoint.WaypointType.Other:
                    otherWaypoints.Add(waypoints[i]);
                    continue;
                case Waypoint.WaypointType.Tags:
                    tagsWaypoints.Add(waypoints[i]);
                    continue;
            }
        }
        waypoints.Clear();
    }
    
    public void ClearLists()
    {
        enemyWaypoints.Clear();
        playerWaypoints.Clear();
        bossWaypoints.Clear();
        itemWaypoints.Clear();
        otherWaypoints.Clear();
        tagsWaypoints.Clear();
    }

    private List<Waypoint> GetWaypointListByType(Waypoint.WaypointType type)
    {
        switch (type)
        {
            case Waypoint.WaypointType.enemySpawn:
                return enemyWaypoints;
            case Waypoint.WaypointType.PlayerSpawn:
                return playerWaypoints;
            case Waypoint.WaypointType.BossSpawn:
                return bossWaypoints;
            case Waypoint.WaypointType.items:
                return itemWaypoints;
            case Waypoint.WaypointType.Other:
                return otherWaypoints;
            case Waypoint.WaypointType.Tags:
                return tagsWaypoints;
        }
        return null;
    }


    private Waypoint PickWaypointInLottery(Lottery lot)
    {
        if (lot.Count > 0)
        {
            Waypoint result = (Waypoint)lot.Pick();
            return result.alreadyConverted ? result : result.Convert();
        }
        else
            return null;
    }

    private List<Waypoint> ConvertAllNotConverted(List<Waypoint> waypointsNotConverted)
    {
        for (int i = 0; i < waypointsNotConverted.Count; i++)
        {
            if (!waypointsNotConverted[i].alreadyConverted)
                waypointsNotConverted[i].Convert();
        }
        return waypointsNotConverted;
    }

    /// <summary>
    /// Retourne un waypoint aleatoire parmis la liste disponible en fonction de votre type
    /// </summary>
    public Waypoint GetRandomSpawnPoint(Waypoint.WaypointType type)
    {
        Lottery lottery = new Lottery();
        List<Waypoint> currentWaypoints = GetWaypointListByType(type);

        for (int i = 0; i < currentWaypoints.Count; i++)
            lottery.Add(currentWaypoints[i], 1);

        return PickWaypointInLottery(lottery);
    }

    /// <summary>
    /// Retourne une quantite aleatoire de waypoints selon votre type
    /// </summary>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, int amount)
    {
        List<Waypoint> spawnpoints = GetWaypointListByType(type);
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        if (amount > spawnpoints.Count)
            return spawnpoints;

        int winner;

        for (int i = 0; i < amount; i++)
        {
            bool winnerNotFound;
            do
            {
                winnerNotFound = false;
                winner = Random.Range(0, spawnpoints.Count - 1);
                for (int j = 0; j < spawnpointsResult.Count; j++)
                {
                    // S'il s'agit d'un spawnpoint qui a deja gagner
                    if (spawnpoints[winner] == spawnpointsResult[j])
                        winnerNotFound = true;
                }
            } while (winnerNotFound);

            spawnpointsResult.Add(spawnpoints[winner]);
        }

        return ConvertAllNotConverted(spawnpointsResult);
    }

    /// <summary>
    /// Retourne une quantite aleatoire de waypoints selon votre type et dans un intervalle d'index
    /// </summary>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, int amount, int indexStart, int indexEnd)
    {
        List<Waypoint> spawnpoints = GetWaypointListByType(type);
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        if (amount > spawnpoints.Count)
            return spawnpoints;

        int winner;

        for (int i = 0; i < amount; i++)
        {
            bool winnerNotFound;
            do
            {
                winnerNotFound = false;
                winner = Random.Range(0, spawnpoints.Count - 1);
                for (int j = 0; j < spawnpointsResult.Count; j++)
                {
                    // S'il s'agit d'un spawnpoint qui a deja gagner
                    if (spawnpoints[winner] == spawnpointsResult[j])
                        winnerNotFound = true;
                }
            } while (winnerNotFound);

            spawnpointsResult.Add(spawnpoints[winner]);
        }

        return ConvertAllNotConverted(spawnpointsResult);
    }

    /// <summary>
    /// Retourne une quantite aleatoire de waypoints selon votre type et tag
    /// </summary>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, string tag, int amount)
    {
        List<Waypoint> spawnpoints = GetWaypointListByType(type);
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        if (amount > spawnpoints.Count)
            return spawnpoints;

        int winner;

        for (int i = 0; i < amount; i++)
        {
            bool winnerNotFound;
            do
            {
                winnerNotFound = false;
                winner = Random.Range(0, spawnpoints.Count - 1);
                for (int j = 0; j < spawnpointsResult.Count; j++)
                {
                    // S'il s'agit d'un spawnpoint qui a deja gagner
                    if (spawnpoints[winner] == spawnpointsResult[j])
                        winnerNotFound = true;
                }
            } while (winnerNotFound);

            spawnpointsResult.Add(spawnpoints[winner]);
        }

        return ConvertAllNotConverted(spawnpointsResult);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Mapping))]
public class Mapping_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Build Lists"))
        {
            (target as Mapping).BuildWaypointLists();
        }
        if (GUILayout.Button("Clear Lists"))
        {
            (target as Mapping).ClearLists();
        }
    }
}
#endif