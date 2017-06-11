using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;
using CCC.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mapping : MonoBehaviour, IComparer<Waypoint>
{
    [SerializeField, Header("Fill")]
    private List<Waypoint> unfilteredWaypoints;

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
        for (int i = 0; i < unfilteredWaypoints.Count; i++)
        {
            switch (unfilteredWaypoints[i].GetWaypointType())
            {
                case Waypoint.WaypointType.enemySpawn:
                    enemyWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
                case Waypoint.WaypointType.PlayerSpawn:
                    playerWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
                case Waypoint.WaypointType.BossSpawn:
                    bossWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
                case Waypoint.WaypointType.items:
                    itemWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
                case Waypoint.WaypointType.Other:
                    otherWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
                case Waypoint.WaypointType.Tags:
                    tagsWaypoints.Add(unfilteredWaypoints[i]);
                    continue;
            }
        }
        unfilteredWaypoints.Clear();

        //Sort lists ! (du pos.y le plus bas au plus haut)
        enemyWaypoints.Sort(this);
        playerWaypoints.Sort(this);
        bossWaypoints.Sort(this);
        itemWaypoints.Sort(this);
        otherWaypoints.Sort(this);
        tagsWaypoints.Sort(this);
    }

    public int Compare(Waypoint x, Waypoint y)
    {
        if (x.RawPosition.y > y.RawPosition.y)
            return 1;
        else if (x.RawPosition.y > y.RawPosition.y)
            return -1;
        else
            return 0;
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

    #region Private

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
            return result;
        }
        else
            return null;
    }

    //private List<Waypoint> AdjustAllNotadjusted(List<Waypoint> waypointsNotConverted)
    //{
    //    for (int i = 0; i < waypointsNotConverted.Count; i++)
    //    {
    //        if (!waypointsNotConverted[i].alreadyConverted)
    //            waypointsNotConverted[i].AdjustToMap();
    //    }
    //    return waypointsNotConverted;
    //}
    #endregion

    #region Public
    public List<Waypoint> GetWaypoints(Waypoint.WaypointType type)
    {
        switch (type)
        {
            case Waypoint.WaypointType.enemySpawn:
                return new List<Waypoint>(enemyWaypoints);
            case Waypoint.WaypointType.PlayerSpawn:
                return new List<Waypoint>(playerWaypoints);
            case Waypoint.WaypointType.BossSpawn:
                return new List<Waypoint>(bossWaypoints);
            case Waypoint.WaypointType.items:
                return new List<Waypoint>(itemWaypoints);
            case Waypoint.WaypointType.Other:
                return new List<Waypoint>(otherWaypoints);
            case Waypoint.WaypointType.Tags:
                return new List<Waypoint>(tagsWaypoints);
        }
        return null;
    }
    public List<Waypoint> GetWaypoints(Waypoint.WaypointType type, float minHeight, float maxHeight)
    {
        List<Waypoint> allWps = GetWaypoints(type);
        List<Waypoint> filteredWps = new List<Waypoint>();

        for (int i = 0; i < allWps.Count; i++)
        {
            //On peut 'break' ici parce que les liste sont ordonn�s en ordre de position.y
            if (allWps[i].AdjustedPosition.y > maxHeight)
                break;

            if (allWps[i].AdjustedPosition.y > minHeight)
            {
                filteredWps.Add(allWps[i]);
            }
        }
        return filteredWps;
    }

    public Waypoint GetRandomWaypoint(Waypoint.WaypointType type)
    {
        List<Waypoint> waypoints = GetWaypointListByType(type);

        //Pas besoin d'une lotterie
        return waypoints[Random.Range(0, waypoints.Count)];
    }
    public Waypoint GetRandomWaypoint(Waypoint.WaypointType type, float minHeight, float maxHeight)
    {
        List<Waypoint> filteredWps = GetWaypoints(type, minHeight, maxHeight);

        //Pas besoin d'une lotterie
        return filteredWps[Random.Range(0, filteredWps.Count)];
    }

    public List<Waypoint> GetMultipleRandomWaypoints(Waypoint.WaypointType type, int amount)
    {
        List<Waypoint> newList = GetWaypoints(type);
        int count = newList.Count;

        //On en prend beaucoup ou peu ?
        if (amount > count / 2)
        {
            if (amount >= count) // smart
                return newList;

            //On en enleve
            for (int i = amount; i < count; i++)
            {
                newList.RemoveAt(Random.Range(0, newList.Count));
            }

            return newList;
        }
        else
        {
            //On en prend peu
            List<Waypoint> smallerList = new List<Waypoint>(amount);

            //On en rajoute
            for (int i = 0; i < amount; i++)
            {
                smallerList[i] = newList[Random.Range(0, newList.Count)];
            }

            return smallerList;
        }
    }
    public List<Waypoint> GetMultipleRandomWaypoints(Waypoint.WaypointType type, int amount, float minHeight, float maxHeight)
    {
        List<Waypoint> allWps = GetWaypoints(type, minHeight, maxHeight);
        int count = allWps.Count;

        //On en prend beaucoup ou peu ?
        if (amount > count / 2)
        {
            if (amount >= count) // smart
                return allWps;

            //On en enleve
            for (int i = amount; i < count; i++)
            {
                allWps.RemoveAt(Random.Range(0, allWps.Count));
            }

            return allWps;
        }
        else
        {
            //On en prend peu
            List<Waypoint> smallerList = new List<Waypoint>(amount);

            //On en rajoute
            for (int i = 0; i < amount; i++)
            {
                smallerList[i] = allWps[Random.Range(0, allWps.Count)];
            }

            return smallerList;
        }
    }
    
    // J'ai enlever des fonctions. Si on veut les ravoir, il faut les refaire avec le temps d'exec en t�te
    // Donc ne PAS utiliser de while(pasR�ussi){...} avec un random � l'int�rieur.
    // �a peut potentiellement prendre PLEIN de temps.

    #endregion
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