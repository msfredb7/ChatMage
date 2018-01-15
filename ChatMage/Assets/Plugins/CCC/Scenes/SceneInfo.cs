using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif

[CreateAssetMenu(fileName = "SI_NewScene", menuName = "CCC/Scenes/Scene Info")]
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
