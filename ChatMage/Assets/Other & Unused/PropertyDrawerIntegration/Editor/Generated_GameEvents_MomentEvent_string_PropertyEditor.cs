using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(GameEvents.MomentEvent_string))]
    public class Generated_GameEvents_MomentEvent_string_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_GameEvents_MomentEvent_string_MonoBehaviourStorage, GameEvents.MomentEvent_string> {
        public override bool CanEdit(Type type) {
            return typeof(GameEvents.MomentEvent_string).IsAssignableFrom(type);
        }
    }
}
