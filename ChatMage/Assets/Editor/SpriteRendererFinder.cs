using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class SpriteRendererFinder : EditorWindow
{
    Sprite theSpr = null;

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
        if (sprr != null && sprr.sprite == theSpr)
            list.Add(obj.gameObject);

        //Check image
        Image image = obj.GetComponent<Image>();
        if (image != null && image.sprite == theSpr)
            list.Add(obj.gameObject);

        foreach (Transform child in obj.transform)
        {
            RecursiveSearch(child, ref list);
        }
    }
}
