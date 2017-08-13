using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FullInspector;

[ExecuteInEditMode]
public class TaggedObject : BaseBehavior
{
    public string[] tags;

    [InspectorButton]
    void ApplyToMapping()
    {
        if (!Application.isPlaying)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.isLoaded)
            {
                Map map = scene.FindRootObject<Map>();
                if (map != null)
                {
                    Mapping mapping = scene.FindRootObject<Map>().mapping;
                    if (mapping != null)
                    {
                        ClearMappingOfMe(mapping);
                        mapping.unfilteredTaggedObjects.Add(this);
                        mapping.FilterTaggedObjects();
                        Debug.Log("Applied to mapping successfully...");
                    }
                    else
                    {
                        Debug.LogError("A Mapping script needs to be linked to Map.");
                    }
                }
                else
                {
                    Debug.LogError("Map script not found.");
                }
            }
        }
    }

    void ClearMappingOfMe(Mapping mapping)
    {
        bool removed = true;
        while (removed)
        {
            removed = false;
            foreach (KeyValuePair<string, List<TaggedObject>> item in mapping.taggedObjects)
            {
                item.Value.RemoveNulls();
                item.Value.Remove(this);
                if (item.Value.Count == 0)
                {
                    mapping.taggedObjects.Remove(item.Key);
                    removed = true;
                    break;
                }
            }
        }
    }


    [InspectorButton]
    void RemoveFromMapping()
    {
        RemoveFromMapping("Removed from mapping successfully...");
    }

    void OnDestroy()
    {
        if (!Application.isPlaying)
        {
            RemoveFromMapping("Removed from mapping automatically...");
        }
    }
    void RemoveFromMapping(string log)
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.isLoaded)
        {
            Map map = scene.FindRootObject<Map>();
            if (map != null)
            {
                if (map.mapping != null)
                {
                    ClearMappingOfMe(map.mapping);
                    Debug.Log(log);
                }
            }
        }
    }
}
