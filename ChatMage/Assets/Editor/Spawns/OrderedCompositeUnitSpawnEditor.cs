using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;
using UnityEditor;

[CustomBehaviorEditor(typeof(OrderedCompositeUnitSpawn))]
public class OrderedCompositeUnitSpawnEditor : BehaviorEditor<OrderedCompositeUnitSpawn>
{

    protected override void OnEdit(Rect rect, OrderedCompositeUnitSpawn behavior, fiGraphMetadata metadata)
    {
        EditorGUI.BeginChangeCheck();
        DefaultBehaviorEditor.Edit(rect, behavior, metadata);
        if (EditorGUI.EndChangeCheck())
        {
            int problematicIndex = -1;
            if (behavior.IsThereALoop(out problematicIndex, behavior))
            {
                behavior.RemoveSubSpawnAt(problematicIndex);
                Debug.LogError("Loop detected within Composite Unit Spawns");
            }
        }
    }

    protected override float OnGetHeight(OrderedCompositeUnitSpawn behavior, fiGraphMetadata metadata)
    {
        return DefaultBehaviorEditor.GetHeight(behavior, metadata);
    }

    protected override void OnSceneGUI(OrderedCompositeUnitSpawn behavior)
    {
    }
}

