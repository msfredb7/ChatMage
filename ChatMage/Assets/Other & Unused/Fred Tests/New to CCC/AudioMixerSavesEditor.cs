using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

[CustomEditor(typeof(AudioMixerSaves))]
public class AudioMixerSavesEditor : Editor
{
    SerializedProperty _fileName;
    SerializedProperty _mixer;
    SerializedProperty _defaultSettings;
    SerializedProperty _loadOnEnable;
    private GUIStyle runtimeStyle;

    void CheckResources()
    {
        if (_fileName == null)
            _fileName = serializedObject.FindProperty("fileName");
        if (_mixer == null)
            _mixer = serializedObject.FindProperty("mixer");
        if (_defaultSettings == null)
            _defaultSettings = serializedObject.FindProperty("defaultSettings");
        if (_loadOnEnable == null)
            _loadOnEnable = serializedObject.FindProperty("loadOnInit");

        if (runtimeStyle == null)
        {
            runtimeStyle = new GUIStyle(EditorStyles.boldLabel);
            runtimeStyle.normal.textColor = new Color(0.65f, 0f, 0f);
        }
    }

    public override void OnInspectorGUI()
    {
        CheckResources();
        var obj = (AudioMixerSaves)target;

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Open Explorer At Location"))
        {
            string path = Application.persistentDataPath.Replace('/', '\\');

            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_fileName, true);
        EditorGUILayout.LabelField(".dat", GUILayout.Width(45));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_loadOnEnable, true);
        EditorGUILayout.PropertyField(_mixer, true);
        EditorGUILayout.PropertyField(_defaultSettings, true);

        EditorGUILayout.Space();
        if (GUILayout.Button("Revert to defaults"))
        {
            obj.SetDefaults();
        }
        if (GUILayout.Button("Save to disk"))
        {
            obj.Save();
        }
        if (GUILayout.Button("Load from disk"))
        {
            obj.Load();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        if (Application.isPlaying)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("RUNTIME", runtimeStyle);

            AudioMixerSaves.Settings settings = obj.GetCurrentSettings();

            EditorGUI.BeginChangeCheck();
            DrawChannel("Master", ref settings.master);
            DrawChannel("Voice", ref settings.voice);
            DrawChannel("SFX", ref settings.sfx);
            DrawChannel("Music", ref settings.music);
            if (EditorGUI.EndChangeCheck())
            {
                obj.SetNewSettings(settings);
            }
        }
    }

    private void DrawChannel(string label, ref AudioMixerSaves.Channel channel)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label);
        channel.muted = EditorGUILayout.Toggle("Muted", channel.muted);
        channel.dbBoost = EditorGUILayout.FloatField("Db Boost", channel.dbBoost);
    }
}
