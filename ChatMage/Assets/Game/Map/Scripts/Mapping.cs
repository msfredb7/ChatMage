using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;
using CCC.Utility;
using FullInspector;
using FullSerializer;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mapping : BaseBehavior, IComparer<Waypoint>
{
    [SerializeField, InspectorCategory("Fill")]
    public List<Waypoint> unfilteredWaypoints;

    [SerializeField, InspectorCategory("Result")]
    private List<Waypoint> enemyWaypoints;
    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    private List<Waypoint> playerWaypoints;
    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    private List<Waypoint> bossWaypoints;
    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    private List<Waypoint> itemWaypoints;
    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    private List<Waypoint> otherWaypoints;
    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    private Dictionary<string, List<Waypoint>> taggedWaypoints;

    [InspectorButton(), InspectorCategory("Fill")]
    public void BuildWaypointLists()
    {
        for (int i = 0; i < unfilteredWaypoints.Count; i++)
        {
            if (unfilteredWaypoints[i].useTag)
            {
                //On l'ajoute dans tous les categories
                for (int u = 0; u < unfilteredWaypoints[i].tags.Length; u++)
                {
                    string tag = unfilteredWaypoints[i].tags[u];
                    if (taggedWaypoints.ContainsKey(tag))
                    {
                        //Ajout a la liste
                        if (!taggedWaypoints[tag].Contains(unfilteredWaypoints[i]))
                            taggedWaypoints[tag].Add(unfilteredWaypoints[i]);
                    }
                    else
                    {
                        //Nouvelle liste
                        taggedWaypoints.Add(tag, new List<Waypoint>());
                        taggedWaypoints[tag].Add(unfilteredWaypoints[i]);
                    }
                }
            }
            else
            {
                switch (unfilteredWaypoints[i].Type)
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
                }
            }
        }


        unfilteredWaypoints.Clear();

        //ON NE LE FAIT PLUS. ÇA FUCK L'ORDRE
        //Sort lists ! (du pos.y le plus bas au plus haut)
        //enemyWaypoints.Sort(this);
        //playerWaypoints.Sort(this);
        //bossWaypoints.Sort(this);
        //itemWaypoints.Sort(this);
        //otherWaypoints.Sort(this);
        //tagsWaypoints.Sort(this);
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

    [InspectorButton(), InspectorCategory("Result")]
    public void ClearLists()
    {
        enemyWaypoints.Clear();
        playerWaypoints.Clear();
        bossWaypoints.Clear();
        itemWaypoints.Clear();
        otherWaypoints.Clear();
        if (taggedWaypoints != null)
            taggedWaypoints.Clear();
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
        }
        return null;
    }
    private List<Waypoint> GetWaypointListByTag(string tag)
    {
        try
        {
            return taggedWaypoints[tag];
        }
        catch
        {
            return null;
        }
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

    public List<Waypoint> GetWaypoints(string tag)
    {
        List<Waypoint> list = GetWaypointListByTag(tag);
        if (list != null)
            return new List<Waypoint>(GetWaypointListByTag(tag));
        else
            return new List<Waypoint>();
    }
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
        }
        return null;
    }
    public List<Waypoint> GetWaypoints(string tag, float minHeight, float maxHeight)
    {
        return FilterWaypointsToNewList(GetWaypoints(tag), minHeight, maxHeight);
    }
    public List<Waypoint> GetWaypoints(Waypoint.WaypointType type, float minHeight, float maxHeight)
    {
        return FilterWaypointsToNewList(GetWaypointListByType(type), minHeight, maxHeight);
    }

    public Waypoint GetRandomWaypoint(string tag)
    {
        List<Waypoint> waypoints = GetWaypoints(tag);

        if (waypoints.Count == 0)
            return null;

        return waypoints[Random.Range(0, waypoints.Count)];
    }
    public Waypoint GetRandomWaypoint(Waypoint.WaypointType type)
    {
        List<Waypoint> waypoints = GetWaypointListByType(type);

        if (waypoints.Count == 0)
            return null;

        return waypoints[Random.Range(0, waypoints.Count)];
    }
    public Waypoint GetRandomWaypoint(string tag, float minHeight, float maxHeight)
    {
        List<Waypoint> filteredWps = GetWaypoints(tag, minHeight, maxHeight);

        if (filteredWps.Count == 0)
            return null;

        return filteredWps[Random.Range(0, filteredWps.Count)];
    }
    public Waypoint GetRandomWaypoint(Waypoint.WaypointType type, float minHeight, float maxHeight)
    {
        List<Waypoint> filteredWps = GetWaypoints(type, minHeight, maxHeight);

        if (filteredWps.Count == 0)
            return null;

        return filteredWps[Random.Range(0, filteredWps.Count)];
    }

    public List<Waypoint> GetMultipleRandomWaypoints(string tag, int amount)
    {
        return GetRandomAmong(GetWaypoints(tag), amount);
    }
    public List<Waypoint> GetMultipleRandomWaypoints(Waypoint.WaypointType type, int amount)
    {
        return GetRandomAmong(GetWaypoints(type), amount);
    }
    public List<Waypoint> GetMultipleRandomWaypoints(string tag, int amount, float minHeight, float maxHeight)
    {
        return GetRandomAmong(GetWaypoints(tag, minHeight, maxHeight), amount);
    }
    public List<Waypoint> GetMultipleRandomWaypoints(Waypoint.WaypointType type, int amount, float minHeight, float maxHeight)
    {
        return GetRandomAmong(GetWaypoints(type, minHeight, maxHeight), amount);
    }

    public List<Waypoint> FilterWaypointsToNewList(List<Waypoint> allWps, float minHeight, float maxHeight)
    {
        List<Waypoint> filteredWps = new List<Waypoint>();

        for (int i = 0; i < allWps.Count; i++)
        {
            if (allWps[i].AdjustedPosition.y > minHeight && allWps[i].AdjustedPosition.y < maxHeight)
            {
                filteredWps.Add(allWps[i]);
            }
        }
        return filteredWps;
    }

    public List<Waypoint> GetRandomAmong(List<Waypoint> newList, int amount)
    {
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

    // J'ai enlever des fonctions. Si on veut les ravoir, il faut les refaire avec le temps d'exec en t�te
    // Donc ne PAS utiliser de while(pasR�ussi){...} avec un random � l'int�rieur.
    // �a peut potentiellement prendre PLEIN de temps.

    #endregion
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Mapping))]
//public class Mapping_Editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("Build Lists"))
//        {
//            (target as Mapping).BuildWaypointLists();
//        }
//        if (GUILayout.Button("Clear Lists"))
//        {
//            (target as Mapping).ClearLists();
//        }
//    }
//}
//#endif