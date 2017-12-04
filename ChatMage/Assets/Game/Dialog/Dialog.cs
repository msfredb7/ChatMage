using FullInspector;

namespace Dialoguing
{
    public class Dialog : BaseScriptableObject
    {
        [InspectorDisabled]
        public int ID = 0;

        public DialogShowType showType = DialogShowType.AlwaysShown;
        public bool pauseGame = true;
        public Reply[] replies;
    }    
}