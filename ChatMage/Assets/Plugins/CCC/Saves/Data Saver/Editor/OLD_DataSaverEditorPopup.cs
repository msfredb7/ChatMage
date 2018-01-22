
using UnityEngine;
using UnityEditor;
using System;

public class OLD_DataSaverEditorPopup : EditorWindow
{
    private OLD_DataSaverEditor.DataType dataType;
    private OLD_DataSaver.Type chosenCategory;
    private OLD_DataSaver gameSaves;

    private int intValue = 0;
    private bool boolValue = false;
    private float floatValue = 0;
    private string stringValue = "";
    private string keyName = "";
    private Action onAdd;

    public static void Popup(OLD_DataSaver gameSaves, OLD_DataSaver.Type chosenCategory, OLD_DataSaverEditor.DataType dataType, Vector2 position, Action onAdd)
    {
        OLD_DataSaverEditorPopup window = ScriptableObject.CreateInstance<OLD_DataSaverEditorPopup>();
        window.position = new Rect(position.x, position.y, 250, 88);
        window.Init(gameSaves, chosenCategory, dataType, onAdd);
        window.ShowPopup();
    }

    void Init(OLD_DataSaver gameSaves, OLD_DataSaver.Type chosenCategory, OLD_DataSaverEditor.DataType dataType, Action onAdd)
    {
        this.gameSaves = gameSaves;
        this.chosenCategory = chosenCategory;
        this.dataType = dataType;
        this.onAdd = onAdd;
    }

    void OnGUI()
    {
        this.DrawWindowColor(new Color(.65f, .65f, .65f));

        EditorGUILayout.LabelField(chosenCategory.ToString() + " - "  + dataType.ToString());
        EditorGUILayout.Space();

        GUILayoutOption firstBoxWidth = GUILayout.Width((position.width / 2) - 7);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Key", EditorStyles.boldLabel, firstBoxWidth);
        EditorGUILayout.LabelField("Value", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        keyName = EditorGUILayout.TextField(keyName, firstBoxWidth);

        switch (dataType)
        {
            default:
            case OLD_DataSaverEditor.DataType.Object:
            case OLD_DataSaverEditor.DataType.All:
                Debug.LogError("Error Type");
                Close();
                break;
            case OLD_DataSaverEditor.DataType.Int:
                intValue = EditorGUILayout.IntField(intValue);
                break;
            case OLD_DataSaverEditor.DataType.Bool:
                boolValue = EditorGUILayout.Toggle(boolValue);
                break;
            case OLD_DataSaverEditor.DataType.Float:
                floatValue = EditorGUILayout.FloatField(floatValue);
                break;
            case OLD_DataSaverEditor.DataType.String:
                stringValue = EditorGUILayout.TextField(stringValue);
                break;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("Cancel", GUILayout.Width(60)))
            Close();

        if (keyName == "")
            GUI.enabled = false;
        if (GUILayout.Button("Add"))
        {
            switch (dataType)
            {
                default:
                case OLD_DataSaverEditor.DataType.All:
                case OLD_DataSaverEditor.DataType.Object:
                    break;
                case OLD_DataSaverEditor.DataType.Int:
                    gameSaves.SetInt(chosenCategory, keyName, intValue);
                    break;
                case OLD_DataSaverEditor.DataType.Bool:
                    gameSaves.SetBool(chosenCategory, keyName, boolValue);
                    break;
                case OLD_DataSaverEditor.DataType.Float:
                    gameSaves.SetFloat(chosenCategory, keyName, floatValue);
                    break;
                case OLD_DataSaverEditor.DataType.String:
                    gameSaves.SetString(chosenCategory, keyName, stringValue);
                    break;
            }

            if (onAdd != null)
                onAdd();

            Close();
        }
        GUI.enabled = true;

        EditorGUILayout.EndHorizontal();
    }
}