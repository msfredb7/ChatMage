using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(UnityObjectReference))]
    public class Generated_UnityObjectReference_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_UnityObjectReference_MonoBehaviourStorage, UnityObjectReference> {
        public override bool CanEdit(Type type) {
            return typeof(UnityObjectReference).IsAssignableFrom(type);
        }
    }
}
