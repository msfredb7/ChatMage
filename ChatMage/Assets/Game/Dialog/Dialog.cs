using FullInspector;
using System.Collections.Generic;

namespace Dialoguing
{
    public class Dialog : BaseScriptableObject
    {
        [InspectorDisabled]
        public int ID = 0;


        public SkipFlags skipFlags = 0;
        public bool pauseGame = true;
        public Reply[] replies;



        public string IDToString()
        {
            return ID.ToString().PadLeft(6, '0');
        }
    }
}