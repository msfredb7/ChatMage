using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AudioPlayable : ScriptableObject
{
    public abstract void PlayOn(AudioSource audioSource);
    public abstract void PlayLoopedOn(AudioSource audioSource, bool multiplyVolume = false);
    public abstract void GetClipAndVolume(out AudioClip clip, out float volume);
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioPlayable), true)]
public class AudioPlayableEditor : Editor
{
    [SerializeField] private AudioSource _previewer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioPlayable playable = target as AudioPlayable;

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

        GUILayoutOption largeHeight = GUILayout.Height(18 * 2 + 3);

        GUILayout.BeginHorizontal(largeHeight);

        GUI.enabled = _previewer.isPlaying;
        if (GUILayout.Button("Stop", largeHeight))
        {
            _previewer.Stop();
        }
        GUI.enabled = true;

        GUILayout.BeginVertical();
        if (GUILayout.Button("Preview"))
        {
            playable.PlayOn(_previewer);
        }
        if (GUILayout.Button("Preview looped"))
        {
            if (_previewer.isPlaying)
                _previewer.Stop();
            playable.PlayLoopedOn(_previewer);
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
    }

    public void OnEnable()
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(_previewer.gameObject);
    }
}
#endif