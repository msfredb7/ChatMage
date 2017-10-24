using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class SpriteRendererFinder : EditorWindow
{
    Sprite theSpr = null;

    bool checkLayer = false;
    string layer = "Default";

    bool checkSortOrder = false;
    int minOrder = 0;
    int maxOrder = 100;

    bool checkZ = false;
    float minZ;
    float maxZ;

    // Add menu named "My Window" to the Window menu
    [MenuItem("The Time Drifter/Sprite Finder")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SpriteRendererFinder window = (SpriteRendererFinder)EditorWindow.GetWindow(typeof(SpriteRendererFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select all gameobjects with the follow sprite", EditorStyles.boldLabel);
        theSpr = EditorGUILayout.ObjectField(theSpr, typeof(Sprite), true) as Sprite;

        checkLayer = EditorGUILayout.Toggle("Check layer", checkLayer);
        if (checkLayer)
        {
            layer = EditorGUILayout.TextField("Layer name", layer);
        }

        checkSortOrder = EditorGUILayout.Toggle("Check Sort Order", checkSortOrder);
        if (checkSortOrder)
        {
            minOrder = EditorGUILayout.IntField("Min", minOrder);
            maxOrder = EditorGUILayout.IntField("Max", maxOrder);
        }

        checkZ = EditorGUILayout.Toggle("Check Z Position", checkZ);
        if (checkZ)
        {
            minZ = EditorGUILayout.FloatField("Min", minZ);
            maxZ = EditorGUILayout.FloatField("Max", maxZ);
        }

        if (GUILayout.Button("Search in scene"))
        {
            GameObject[] rootObjs = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            List<GameObject> lesTraitres = new List<GameObject>();
            for (int i = 0; i < rootObjs.Length; i++)
            {
                RecursiveSearch(rootObjs[i].transform, ref lesTraitres);
            }

            Selection.objects = lesTraitres.ToArray();
            Debug.Log("Total found: " + lesTraitres.Count);
        }
    }

    void RecursiveSearch(Transform obj, ref List<GameObject> list)
    {
        //Check sprite renderer
        SpriteRenderer sprr = obj.GetComponent<SpriteRenderer>();
        if (sprr != null && Match(sprr))
            list.Add(obj.gameObject);

        //Check image
        Image image = obj.GetComponent<Image>();
        if (image != null && Match(image))
            list.Add(obj.gameObject);

        foreach (Transform child in obj.transform)
        {
            RecursiveSearch(child, ref list);
        }
    }

    bool Match(Image img)
    {
        return img.sprite == theSpr;
    }
    bool Match(SpriteRenderer sprR)
    {
        return sprR.sprite == theSpr
            && CheckLayer(sprR.sortingLayerName) 
            && CheckSortOrder(sprR.sortingOrder) 
            && CheckZ(sprR.transform);
    }

    bool CheckLayer(string layer)
    {
        return !checkLayer || this.layer == layer;
    }

    bool CheckSortOrder(int order)
    {
        return !checkSortOrder || (order >= minOrder && order <= maxOrder);
    }

    bool CheckZ(Transform tr)
    {
        float z = tr.position.z;
        return !checkZ || (z >= minZ && z <= maxZ);
    }
}
