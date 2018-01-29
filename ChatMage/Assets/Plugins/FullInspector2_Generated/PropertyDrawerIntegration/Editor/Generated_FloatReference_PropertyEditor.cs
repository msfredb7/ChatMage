using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(FloatReference))]
    public class Generated_FloatReference_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_FloatReference_MonoBehaviourStorage, FloatReference> {
        public override bool CanEdit(Type type) {
            return typeof(FloatReference).IsAssignableFrom(type);
        }
    }
}
