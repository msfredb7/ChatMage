using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using CCC.Manager;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Scene Info", menuName = "Scenes/Scene Info")]
public class SceneInfo : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset scene;
    public SceneAsset GetEditorScene()
    {
        return scene;
    }
    public void RefreshSceneName()
    {
        sceneName = scene.name;
    }
#endif

    [SerializeField, ReadOnly]
    private string sceneName;
    public string SceneName { get { return sceneName; } }

    [SerializeField, Header("Defaults")] private LoadSceneMode loadMode = LoadSceneMode.Single;
    [SerializeField] private bool allowMultiple;

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

    public override void OnInspectorGUI()
    {
        Color guiColor = GUI.color;
        if (_target.GetEditorScene() != null && _target.GetEditorScene().name != _target.SceneName)
        {
            GUI.color = new Color(1, 0.5f, 0.5f);
        }
        else
        {
            GUI.enabled = false;
        }

        if (GUILayout.Button("Refresh Name"))
        {
            _target.RefreshSceneName();
            EditorUtility.SetDirty(_target);
        }

        GUI.enabled = true;
        GUI.color = guiColor;

        base.OnInspectorGUI();
    }
}
#endif
