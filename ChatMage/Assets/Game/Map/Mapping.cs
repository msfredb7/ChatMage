using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;
using CCC.Utility;

public class Mapping : MonoBehaviour {

    private float mapHeight;
    private float mapWidth;

    private float limitTop;
    private float limitBottom;
    private float limitRight;
    private float limitLeft;

    [SerializeField]
    private List<Waypoint> waypoints;

    public void Init(float height, float width)
    {
        Debug.Log("Mapping Init");
        mapHeight = height;
        mapWidth = width;
    }

    public void SetOffsets(float top, float bottom, float right, float left)
    {
        Debug.Log("Setting Offsets in the map");
        if (top >= mapHeight)
            limitTop = top;
        else
            limitTop = mapHeight;

        if (bottom >= 0)
            limitBottom = bottom;
        else
            limitBottom = 0;

        if (right >= mapWidth)
            limitRight = right;
        else
            limitRight = mapWidth;

        if (left >= 0)
            limitLeft = left;
        else
            limitLeft = 0;
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
        for(int i = 0; i < waypointsNotConverted.Count; i++)
        {
            if (!waypointsNotConverted[i].alreadyConverted)
                waypointsNotConverted[i].Convert();
        }
        return waypointsNotConverted;
    }

    /// <summary>
    /// Retourne un waypoint aleatoire parmis la liste disponible en fonction de votre type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Waypoint GetRandomSpawnPoint(Waypoint.WaypointType type)
    {
        Lottery lottery = new Lottery();
        for(int i = 0; i < waypoints.Count; i++)
        {
            if(waypoints[i].GetWaypointType() == type)
                lottery.Add(waypoints[i],1);
        }
        return PickWaypointInLottery(lottery);
    }

    /// <summary>
    /// Retourne une quantite aleatoire de waypoints selon votre type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, int amount)
    {
        List<Waypoint> spawnpoints = new List<Waypoint>();
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i].GetWaypointType() == type)
                spawnpoints.Add(waypoints[i]);
        }

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
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, int amount, int indexStart, int indexEnd)
    {
        List<Waypoint> spawnpoints = new List<Waypoint>();
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        for (int i = indexStart; i < indexEnd; i++)
        {
            if (waypoints[i].GetWaypointType() == type)
                spawnpoints.Add(waypoints[i]);
        }

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
    /// <param name="type"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public List<Waypoint> GetRandomMultipleSpawnPoint(Waypoint.WaypointType type, string tag, int amount)
    {
        List<Waypoint> spawnpoints = new List<Waypoint>();
        List<Waypoint> spawnpointsResult = new List<Waypoint>();

        for (int i = 0; i < waypoints.Count; i++)
        {
            if ((waypoints[i].GetWaypointType() == type) && waypoints[i].CompareTag(tag))
                spawnpoints.Add(waypoints[i]);
        }

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
