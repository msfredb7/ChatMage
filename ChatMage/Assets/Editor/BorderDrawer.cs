using UnityEditor;
using FullInspector;
using UnityEngine;

[CustomPropertyEditor(typeof(Border))]
public class IntPropertyEditor : PropertyEditor<Border>
{
    public override Border Edit(Rect region, GUIContent label, Border element, fiGraphMetadata metadata)
    {
        Rect leftRegion = new Rect(region.x, region.y, region.width / 2, region.height);
        Rect rightRegion = new Rect(region.x+ region.width / 2, region.y, region.width / 2, region.height);

        element.enabled = EditorGUI.ToggleLeft(leftRegion, label, element.enabled);

        EditorGUIUtility.labelWidth = 55;
        element.padding = EditorGUI.FloatField(rightRegion, "padding", element.padding);

        return element;
    }
    public override float GetElementHeight(GUIContent label, Border element, fiGraphMetadata metadata)
    {
        return EditorStyles.numberField.CalcHeight(label, 1000);
    }
}