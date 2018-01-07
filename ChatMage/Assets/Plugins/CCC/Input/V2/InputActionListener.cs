using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InputActionListener : MonoBehaviour
{
    public InputAction action;
    public bool onPressEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onPress = new UnityEvent();

    public bool onHoldEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onHold = new UnityEvent();

    public bool onReleaseEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onRelease = new UnityEvent();

    private void Update()
    {
        if (onPressEvents && action.GetDown())
            onPress.Invoke();

        if (onHoldEvents && action.Get())
            onHold.Invoke();

        if (onReleaseEvents && action.GetUp())
            onRelease.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputActionListener))]
public class InputActionListenerEditor : Editor
{
    private SerializedProperty _onPress;
    private SerializedProperty _onHold;
    private SerializedProperty _onRelease;
    private InputActionListener _target;

    private void OnEnable()
    {
        _onPress = serializedObject.FindProperty("onPress");
        _onHold = serializedObject.FindProperty("onHold");
        _onRelease = serializedObject.FindProperty("onRelease");
        _target = target as InputActionListener;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        if (_target.onPressEvents)
        {
            EditorGUILayout.PropertyField(_onPress);
        }
        if (_target.onHoldEvents)
        {
            EditorGUILayout.PropertyField(_onHold);
        }
        if (_target.onReleaseEvents)
        {
            EditorGUILayout.PropertyField(_onRelease);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
