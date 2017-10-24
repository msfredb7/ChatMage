using UnityEditor;
using UnityEngine;

namespace FullInspector.Modules.Attributes {
    [CustomAttributePropertyEditor(typeof(InspectorHeaderAttribute), ReplaceOthers = false)]
    public class InspectorHeaderAttributeEditor<T> : AttributePropertyEditor<T, InspectorHeaderAttribute> {
        private const float AboveMargin = 15;
        private const float BelowMargin = 5;
        protected override T Edit(Rect region, GUIContent label, T element, InspectorHeaderAttribute attribute, fiGraphMetadata metadata) {
            region.height -= AboveMargin + BelowMargin;
            region.center -= Vector2.down * AboveMargin;
            GUI.Label(region, attribute.Header, EditorStyles.boldLabel);
            return element;
        }

        protected override float GetElementHeight(GUIContent label, T element, InspectorHeaderAttribute attribute, fiGraphMetadata metadata) {
            return EditorStyles.boldLabel.CalcHeight(label, 100) + BelowMargin + AboveMargin;
        }
    }
}