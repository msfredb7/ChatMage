using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using CCC;
using CCC.Utility;
using FullInspector;
using FullSerializer;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mapping : BaseBehavior
{
    [SerializeField, InspectorCategory("Fill")]
    public List<TaggedObject> unfilteredTaggedObjects;
    [SerializeField, InspectorCategory("Fill")]
    public List<UnitSpawn> unfilteredSpawns;

    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    public Dictionary<string, List<TaggedObject>> taggedObjects;

    [SerializeField, ReadOnly(), InspectorCategory("Result")]
    public Dictionary<string, List<UnitSpawn>> spawns;

    [InspectorButton(), InspectorCategory("Fill")]
    public void Filter()
    {
        FilterTaggedObjects();
        FilterSpawns();
    }

    public void FilterTaggedObjects()
    {
        //Tagged objects
        for (int i = 0; i < unfilteredTaggedObjects.Count; i++)
        {
            for (int u = 0; u < unfilteredTaggedObjects[i].tags.Length; u++)
            {
                string tag = unfilteredTaggedObjects[i].tags[u];

                if (taggedObjects.ContainsKey(tag))
                {
                    //Ajout a la liste
                    if (!taggedObjects[tag].Contains(unfilteredTaggedObjects[i]))
                        taggedObjects[tag].Add(unfilteredTaggedObjects[i]);
                }
                else
                {
                    //Nouvelle liste
                    taggedObjects.Add(tag, new List<TaggedObject>());
                    taggedObjects[tag].Add(unfilteredTaggedObjects[i]);
                }
            }
        }
        unfilteredTaggedObjects.Clear();
    }

    public void FilterSpawns()
    {
        //Spawns
        for (int i = 0; i < unfilteredSpawns.Count; i++)
        {
            for (int u = 0; u < unfilteredSpawns[i].tags.Length; u++)
            {
                string tag = unfilteredSpawns[i].tags[u];

                if (spawns.ContainsKey(tag))
                {
                    //Ajout a la liste
                    if (!spawns[tag].Contains(unfilteredSpawns[i]))
                        spawns[tag].Add(unfilteredSpawns[i]);
                }
                else
                {
                    //Nouvelle liste
                    spawns.Add(tag, new List<UnitSpawn>());
                    spawns[tag].Add(unfilteredSpawns[i]);
                }
            }
        }
        unfilteredSpawns.Clear();
    }

    public void Init(Game instance)
    {
        instance.onGameReady += OnGameReady;
    }

    private void OnGameReady()
    {
        InGameEvents events = Game.instance.events;
        if (spawns != null)
            foreach (KeyValuePair<string, List<UnitSpawn>> spawnGroup in spawns)
            {
                for (int i = 0; i < spawnGroup.Value.Count; i++)
                {
                    spawnGroup.Value[i].Init(events);
                }
            }
    }

    #region Private
    private List<TaggedObject> GetTaggedObjectsListByTag(string tag)
    {
        try
        {
            return taggedObjects[tag];
        }
        catch
        {
            return null;
        }
    }
    private List<UnitSpawn> GetSpawnListByTag(string tag)
    {
        try
        {
            return spawns[tag];
        }
        catch
        {
            return null;
        }
    }
    #endregion

    #region Public

    public List<TaggedObject> GetTaggedObjects(string tag)
    {
        List<TaggedObject> list = GetTaggedObjectsListByTag(tag);
        if (list != null)
            return new List<TaggedObject>(GetTaggedObjectsListByTag(tag));
        else
            return new List<TaggedObject>();
    }
    public TaggedObject GetTaggedObject(string tag)
    {
        List<TaggedObject> list = GetTaggedObjectsListByTag(tag);
        if (list != null)
            return GetTaggedObjectsListByTag(tag)[0];
        else
            return null;
    }

    /// <summary>
    /// Retourne la premiere occurence de spawn avec ce tag
    /// </summary>
    public UnitSpawn GetSpawn(string tag)
    {
        List<UnitSpawn> list = GetSpawnListByTag(tag);
        if (list != null && list.Count > 0)
            return list[0];
        else
            return null;
    }
    public ReadOnlyCollection<UnitSpawn> GetSpawns(string tag)
    {
        List<UnitSpawn> list = GetSpawnListByTag(tag);
        if (list != null)
            return list.AsReadOnly();
        else
            return null;
    }
    public List<UnitSpawn> GetSpawns_NewList(string tag)
    {
        List<UnitSpawn> list = GetSpawnListByTag(tag);
        if (list != null)
            return new List<UnitSpawn>(GetSpawnListByTag(tag));
        else
            return new List<UnitSpawn>();
    }

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