using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(GameEvents.MomentEvent_unit))]
    public class Generated_GameEvents_MomentEvent_unit_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_GameEvents_MomentEvent_unit_MonoBehaviourStorage, GameEvents.MomentEvent_unit> {
        public override bool CanEdit(Type type) {
            return typeof(GameEvents.MomentEvent_unit).IsAssignableFrom(type);
        }
    }
}
