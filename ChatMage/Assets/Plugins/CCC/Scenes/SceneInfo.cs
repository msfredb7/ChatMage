using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using CCC.Manager;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif

[CreateAssetMenu(fileName = "SI_NewScene", menuName = "Scenes/Scene Info")]
public class SceneInfo : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset scene;
    public SceneAsset Editor_GetScene()
    {
        return scene;
    }
    public void Editor_RefreshSceneName()
    {
        sceneName = scene.name;
    }
#endif

    [SerializeField, ReadOnly]
    private string sceneName;
    public string SceneName { get { return sceneName; } }

    [SerializeField, Header("Defaults")] private LoadSceneMode loadMode = LoadSceneMode.Single;
    [SerializeField] private bool allowMultiple = false;

    public void LoadScene()
    {
        LoadScene(null);
    }
    public void LoadScene(Action<Scene> onLoad)
    {
        LoadScene(onLoad, loadMode, !allowMultiple);
    }
    public void LoadScene(Action<Scene> onLoad, LoadSceneMode loadSceneMode, bool unique)
    {
        Scenes.Load(SceneName, loadSceneMode, onLoad, unique);
    }


    public void LoadSceneAsync()
    {
        LoadSceneAsync(null);
    }
    public void LoadSceneAsync(Action<Scene> onLoad)
    {
        LoadSceneAsync(onLoad, loadMode, !allowMultiple);
    }
    public void LoadSceneAsync(Action<Scene> onLoad, LoadSceneMode loadSceneMode, bool unique)
    {
        Scenes.LoadAsync(SceneName, loadSceneMode, onLoad, unique);
    }

    public bool IsActive()
    {
        return Scenes.IsActive(SceneName);
    }

#if UNITY_EDITOR
    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        SceneInfo sceneInfo = obj as SceneInfo;
        if (sceneInfo != null)
        {
            if (sceneInfo.Editor_GetScene() != null)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetOrScenePath(sceneInfo.Editor_GetScene()));
                return true;
            }
        }

        return false; // we did not handle the open
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(SceneInfo))]
public class SceneInfoEditor : Editor
{
    public SceneInfo _target;

    private void OnEnable()
    {
        _target = target as SceneInfo;
    }

    private static void AddToBuild(string assetPath)
    {
        var original = EditorBuildSettings.scenes;
        var newSettings = new EditorBuildSettingsScene[original.Length + 1];
        Array.Copy(original, newSettings, original.Length);

        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(assetPath, true);
        newSettings[newSettings.Length - 1] = sceneToAdd;

        EditorBuildSettings.scenes = newSettings;
    }

    private static void EnableInBuild(int buildIndex)
    {
        var scenes = EditorBuildSettings.scenes;
        scenes[buildIndex].enabled = true;
        EditorBuildSettings.scenes = scenes;
    }

    private static void DisableInBuild(int buildIndex)
    {
        var scenes = EditorBuildSettings.scenes;
        scenes[buildIndex].enabled = false;
        EditorBuildSettings.scenes = scenes;
    }

    private static void PutInFirstInBuild(int buildIndex)
    {
        var original = EditorBuildSettings.scenes;
        var newSettings = new EditorBuildSettingsScene[original.Length];

        EditorBuildSettingsScene theOne = original[buildIndex];

        int u = 1;
        for (int i = 0; i < original.Length; i++)
        {
            if (i != buildIndex)
            {
                newSettings[u] = original[i];
                u++;
            }
        }
        newSettings[0] = theOne;

        EditorBuildSettings.scenes = newSettings;
    }
    private static void RemoveFromBuild(int buildIndex)
    {
        var original = EditorBuildSettings.scenes;
        var newSettings = new EditorBuildSettingsScene[original.Length - 1];

        int u = 0;
        for (int i = 0; i < original.Length; i++)
        {
            if (i != buildIndex)
            {
                newSettings[u] = original[i];
                u++;
            }
        }

        EditorBuildSettings.scenes = newSettings;
    }

    /// <summary>
    ///  -1 -> not in build   0 -> disabled in build   2 -> enabled in build
    /// </summary>
    private int GetBuildState(out string assetPath, out int indexInBuild)
    {
        SceneAsset sceneAsset = _target.Editor_GetScene();
        assetPath = AssetDatabase.GetAssetOrScenePath(sceneAsset);
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].path == assetPath)
            {
                indexInBuild = i;
                if (scenes[i].enabled)
                    return 1;
                else
                    return 0;
            }
        }

        indexInBuild = -1;
        return -1;
    }

    public override void OnInspectorGUI()
    {
        Color guiColor = GUI.color;
        SceneAsset sceneAsset = _target.Editor_GetScene();

        if (sceneAsset != null)
        {
            string assetPath;
            int indexInBuild;

            // -1 -> not in build   0 -> disabled in build   2 -> enabled in build
            int buildState = GetBuildState(out assetPath, out indexInBuild);

            if (buildState == -1)
            {
                GUI.color = new Color(0.6f, 1, 0.6f);
                if (GUILayout.Button("ADD TO BUILD SETTINGS"))
                {
                    AddToBuild(assetPath);
                }
                GUI.color = guiColor;
            }
            else
            {
                GUI.color = new Color(1, 0.6f, 0.6f);
                if (GUILayout.Button("REMOVE FROM BUILD SETTINGS"))
                {
                    RemoveFromBuild(indexInBuild);
                }

                GUI.color = guiColor;
                EditorGUILayout.BeginHorizontal();
                if (EditorGUILayout.Toggle("Included in build", buildState == 1) != (buildState == 1))
                {
                    if (buildState == 1)
                        DisableInBuild(indexInBuild);
                    else
                        EnableInBuild(indexInBuild);
                }

                bool isInFirstPlace = indexInBuild == 0;
                if (isInFirstPlace)
                    GUI.enabled = false;
                if (GUILayout.Button(isInFirstPlace ? "Is in first place" : "Put in first place"))
                {
                    PutInFirstInBuild(indexInBuild);
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
        }

        if (sceneAsset != null && sceneAsset.name != _target.SceneName)
        {
            GUI.color = new Color(0.6f, 1, 0.6f);
        }
        else
        {
            GUI.enabled = false;
        }

        if (GUILayout.Button("Refresh Name"))
        {
            _target.Editor_RefreshSceneName();
            EditorUtility.SetDirty(_target);
        }

        GUI.enabled = true;
        GUI.color = guiColor;

        base.OnInspectorGUI();
    }
}
#endif
