using System;
using FullInspector.Internal;

namespace FullInspector.Generated {
    [CustomPropertyEditor(typeof(CCC.Utility.StatInt))]
    public class Generated_CCC_Utility_StatInt_PropertyEditor : fiGenericPropertyDrawerPropertyEditor<Generated_CCC_Utility_StatInt_MonoBehaviourStorage, CCC.Utility.StatInt> {
        public override bool CanEdit(Type type) {
            return typeof(CCC.Utility.StatInt).IsAssignableFrom(type);
        }
    }
}
