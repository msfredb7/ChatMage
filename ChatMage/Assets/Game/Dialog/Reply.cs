using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

namespace Dialoguing
{
    [System.Serializable]
    public class Reply
    {
        [InspectorTextArea(42)]
        public string message;
        public Effect.Effect[] effects;

        //[InspectorButton]
        public void TestButton()
        {
            bool yes = false;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == '\n')
                    yes = true;
            }
            Debug.Log("Contains 'enters': " + yes);
        }
    }
}
